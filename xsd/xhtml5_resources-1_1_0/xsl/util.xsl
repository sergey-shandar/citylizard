<?xml version="1.0" encoding="UTF-8"?>

<!-- =====================================================================
     XSLT 2 stylesheets for XHTML 1.0, 1.1, 5.0.
     Helper templates and functions.

     Author: Hussein Shafie

     Copyright (c) 2012-2013 Pixware SARL.

     Permission is hereby granted, free of charge, to any person
     obtaining a copy of this software and associated
     documentation files (the "Software"), to deal in the
     Software without restriction, including without limitation
     the rights to use, copy, modify, merge, publish, distribute,
     sublicense, and/or sell copies of the Software, and to
     permit persons to whom the Software is furnished to do so,
     subject to the following conditions:

     The above copyright notice and this permission notice shall
     be included in all copies or substantial portions of the
     Software.

     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY
     KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
     WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
     PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
     OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
     OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
     OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
     SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
     ===================================================================== -->

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xs="http://www.w3.org/2001/XMLSchema"
                xmlns:fo="http://www.w3.org/1999/XSL/Format"
                xmlns:html="http://www.w3.org/1999/xhtml"
                xmlns:u="http://www.xmlmind.com/namespace/xmleditor"
                xmlns:URI="java:com.xmlmind.xmleditext.xhtml.xslt.URI"
                exclude-result-prefixes="xs u URI"
                version="2.0">

  <!-- Common attributes ================================================= -->

  <xsl:template name="commonAttributes">
    <xsl:call-template name="idAttribute"/>
    <xsl:call-template name="localizationAttributes"/>
    <xsl:call-template name="xmlSpaceAttribute"/>
  </xsl:template>

  <xsl:template name="idAttribute">
    <!-- XSL-FO does not mandate its id to be an xs:ID. 
         It just says: "a unique identifier which does contain whitespace". -->
    <xsl:copy-of select="@id"/>
  </xsl:template>

  <xsl:template name="localizationAttributes">
    <!-- XSL treats xml:lang as a shorthand and uses it to set the country and
         language properties. -->
    <xsl:choose>
      <xsl:when test="exists(@lang)">
        <xsl:attribute name="xml:lang" select="string(@lang)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:copy-of select="@xml:lang"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="xmlSpaceAttribute">
    <xsl:if test="@xml:space eq 'preserve'">
      <xsl:attribute name="white-space">pre</xsl:attribute>
    </xsl:if>
  </xsl:template>

  <!-- Helpers =========================================================== -->

  <!-- resolveHref ========== -->

  <xsl:template name="resolveHref">
    <xsl:param name="href" select="''"/>

    <xsl:choose>
      <xsl:when test="matches($href, '^[a-zA-Z][a-zA-Z0-9.+-]*:/')">
        <xsl:value-of select="$href"/>
      </xsl:when>

      <xsl:otherwise>
        <xsl:variable name="baseList1" 
          select="ancestor-or-self::*/@xml:base[. ne '']/string()"/>
        <xsl:variable name="baseList2" select="($baseList1, $href)"/>

        <xsl:variable name="base" 
          select="if (exists(/html:html/html:head/html:base/@href))
                  then string((/html:html/html:head/html:base/@href)[1])
                  else ''"/>

        <xsl:variable name="docBaseURI" 
                      select="if ($base ne '')
                              then resolve-uri($base, base-uri(/))
                              else base-uri(/)"/>

        <xsl:variable name="baseList3" select="($docBaseURI, $baseList2)"/>

        <xsl:variable name="resolved" 
          select="u:doResolveHref($baseList3[1], subsequence($baseList3, 2))"/>

        <xsl:value-of select="$resolved"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:function name="u:doResolveHref" as="xs:string">
    <xsl:param name="base" as="xs:string"/>
    <xsl:param name="uris" as="xs:string*"/>

    <xsl:choose>
      <xsl:when test="empty($uris)">
        <xsl:sequence select="$base"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="newBase" select="resolve-uri($uris[1], $base)"/>
        <xsl:sequence 
          select="u:doResolveHref($newBase, subsequence($uris, 2))"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>

  <!-- asLengthOrPercent, asLength ========== -->

  <xsl:function name="u:asLengthOrPercent" as="xs:string">
    <xsl:param name="value" as="xs:string"/>

    <xsl:variable name="value2" select="normalize-space($value)"/>

    <xsl:choose>
      <xsl:when test="ends-with($value2, '%')">
        <xsl:sequence select="$value2"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:sequence select="u:asLength($value2)"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>

  <xsl:function name="u:asLength" as="xs:string">
    <xsl:param name="value" as="xs:string"/>

    <xsl:variable name="value2" select="normalize-space($value)"/>

    <xsl:variable name="px" as="xs:string?">
      <xsl:choose>
        <xsl:when test="ends-with($value2, 'px')">
          <xsl:variable name="value3"
                        select="substring-before($value2, 'px')"/>
                        
          <xsl:if test="number($value3) ge 0">
            <xsl:sequence select="$value3"/>
          </xsl:if>
        </xsl:when>
        <xsl:when test="number($value2) ge 0">
          <xsl:sequence select="$value2"/>
        </xsl:when>
      </xsl:choose>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="exists($px)">
        <xsl:variable name="pt" 
          select="(number($px) * 72.0) div number($screen-resolution)"/>

        <xsl:sequence select="concat($pt, 'pt')"/>
      </xsl:when>
      <xsl:otherwise>
        <!-- Examples: 12pt, auto, inherit, thick, center, smaller. -->
        <xsl:sequence select="$value2"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>

  <!-- isNumber ========== -->

  <xsl:function name="u:isNumber" as="xs:boolean">
    <xsl:param name="value" as="xs:string"/>

    <xsl:sequence 
      select="($value ne '' and string(number($value)) ne 'NaN')"/>
  </xsl:function>

  <!-- indexOfNode ========== -->

  <xsl:function name="u:indexOfNode" as="xs:integer?">
    <xsl:param name="nodeSequence" as="node()*"/>
    <xsl:param name="searchedNode" as="node()"/>

    <xsl:for-each select="$nodeSequence">
      <xsl:if test=". is $searchedNode">
        <xsl:sequence select="position()"/>
      </xsl:if>
    </xsl:for-each>
  </xsl:function>

  <!-- toAbsoluteURI ========== -->

  <xsl:function name="u:toAbsoluteURI" as="xs:string">
    <xsl:param name="uri" as="xs:string"/>
    
    <xsl:variable name="curDir" select="system-property('user.dir')"/>

    <xsl:choose>
      <xsl:when test="starts-with($uri, '/') or 
                      matches($uri, '^[a-zA-Z][a-zA-Z0-9.+-]*:/') or 
                      $curDir eq ''">
        <xsl:sequence select="$uri"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="curDir2" 
          select="replace(translate($curDir, '\', '/'), ' ', '%20')"/>

        <xsl:variable name="curDir3" 
                      select="if (starts-with($curDir2, '/'))
                              then $curDir2 
                              else concat('/', $curDir2)"/>

        <xsl:variable name="curDir4" 
                      select="if (ends-with($curDir3, '/')) 
                              then $curDir3 
                              else concat($curDir3, '/')"/>

        <xsl:sequence select="concat('file://', $curDir4, $uri)"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>

  <!-- containsBlock ========== -->

  <xsl:function name="u:containsBlock" as="xs:boolean">
    <xsl:param name="container" as="element()"/>

    <xsl:sequence select="exists($container/html:h1 |
                                 $container/html:h2 |
                                 $container/html:h3 |
                                 $container/html:h4 |
                                 $container/html:h5 |
                                 $container/html:h6 |
                                 $container/html:hgroup |
                                 $container/html:div |
                                 $container/html:center |
                                 $container/html:section |
                                 $container/html:nav |
                                 $container/html:article |
                                 $container/html:aside |
                                 $container/html:header |
                                 $container/html:footer |
                                 $container/html:blockquote |
                                 $container/html:address |
                                 $container/html:hr |
                                 $container/html:p |
                                 $container/html:pre |
                                 $container/html:ul |
                                 $container/html:ol |
                                 $container/html:dl |
                                 $container/html:dir |
                                 $container/html:table |
                                 $container/html:figure |
                                 $container/html:fieldset |
                                 $container/html:form |
                                 $container/html:menu |
                                 $container/html:details |
                                 $container/html:script |
                                 $container/html:noscript)"/>
  </xsl:function>

</xsl:stylesheet>
