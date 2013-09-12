<?xml version="1.0" encoding="UTF-8"?>

<!-- =====================================================================
     XSLT 2 stylesheets for XHTML 1.0, 1.1, 5.0.
     Templates for all XHTML elements except table and 
     its descendant elements.

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
                xmlns:xfc="http://www.xmlmind.com/foconverter/xsl/extensions"
                xmlns:html="http://www.w3.org/1999/xhtml"
                xmlns:svg="http://www.w3.org/2000/svg"
                xmlns:mml="http://www.w3.org/1998/Math/MathML"
                xmlns:u="http://www.xmlmind.com/namespace/xmleditor"
                exclude-result-prefixes="xs html u"
                version="2.0">

  <xsl:import href="params.xsl"/>
  <xsl:import href="util.xsl"/>
  <xsl:import href="cssStyles.xsl"/>
  <xsl:import href="pagination.xsl"/>
  <xsl:import href="table.xsl"/>

  <!-- Output ============================================================ -->

  <xsl:output method="xml" encoding="UTF-8" indent="no"/>

  <xsl:preserve-space elements="html:pre html:style html:script"/>

  <!-- Base, abstract and mixin styles =================================== -->

  <xsl:attribute-set name="block-style">
    <xsl:attribute name="space-before">1em</xsl:attribute>
    <xsl:attribute name="space-after">1em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="semi-compact-block-style">
    <xsl:attribute name="space-before">0.5em</xsl:attribute>
    <xsl:attribute name="space-after">0.5em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="compact-block-style">
    <xsl:attribute name="space-before">0em</xsl:attribute>
    <xsl:attribute name="space-after">0em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="display-style">
    <xsl:attribute name="space-before">1.33em</xsl:attribute>
    <xsl:attribute name="space-after">1.33em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="heading-style">
    <xsl:attribute name="font-weight">bold</xsl:attribute>
    <xsl:attribute name="text-align">left</xsl:attribute>
    <xsl:attribute name="hyphenate">false</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="monospace-style">
    <xsl:attribute name="font-family">monospace</xsl:attribute>
    <xsl:attribute name="font-size">90%</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="link-style">
    <xsl:attribute name="text-decoration">underline</xsl:attribute>
    <xsl:attribute name="color">navy</xsl:attribute>
  </xsl:attribute-set>

  <!-- The root element ================================================== -->

  <!-- This xsl:attribute-set and all the other xsl:attribute-sets found
       in the various .xsl files implement the default styles found in the CSS
       stylesheet recommended by the W3C for HTML. 

       These fairly generic styles are intended to be overriden either by the
       CSS stylesheets associated to the HTML document being transformed or by
       user-specified xsl:attribute-sets. -->

  <xsl:attribute-set name="html">
    <xsl:attribute name="font-family" select="$font-family"/>
    <xsl:attribute name="font-size" select="$base-font-size"/>
    <xsl:attribute name="line-height">1.2</xsl:attribute>
    <xsl:attribute name="text-align"
                   select="if ($justified eq 'yes') then 'justify' else 'left'"/>
  </xsl:attribute-set>

  <xsl:template match="html:html">
    <fo:root>
      <xsl:call-template name="localizationAttributes"/>

      <xsl:call-template name="layoutMasterSet"/>

      <fo:page-sequence>
        <xsl:call-template name="configurePageSequence">
          <xsl:with-param name="documentTitle" 
                          select="string(html:head/html:title)"/>
        </xsl:call-template>

        <fo:flow flow-name="xsl-region-body">
          <fo:block xsl:use-attribute-sets="html">
            <xsl:call-template name="commonAttributes"/>
            <xsl:apply-templates/>
          </fo:block>

          <fo:block id="__EOF" hyphenate="false"/>
        </fo:flow>
      </fo:page-sequence>
    </fo:root>
  </xsl:template>

  <!-- Document metadata ================================================= -->
  <!-- title, base, meta, script, style, noscript not rendered. -->

  <xsl:template match="html:head" />

  <xsl:template match="html:style" />

  <!-- Scripting ========================================================= -->
  <!-- not rendered. -->

  <xsl:template match="html:script" />

  <!-- The noscript element must not be used in XHTML5 documents. -->
  <!--LEGACY-->
  <xsl:template match="html:noscript">
    <fo:block>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- Sections ========================================================== -->

  <xsl:attribute-set name="body">
  </xsl:attribute-set>

  <xsl:template match="html:body">
    <fo:block xsl:use-attribute-sets="body">
      <xsl:choose>
        <xsl:when test="$root-id ne ''">
          <xsl:variable name="root" select="//*[@id eq $root-id]"/>
          <xsl:choose>
            <xsl:when test="exists($root)">
              <!-- Apply body CSS styles and not root div CSS styles. -->
              <xsl:call-template name="cssStyles"/>

              <xsl:for-each select="$root">
                <xsl:call-template name="commonAttributes"/>
                <xsl:apply-templates select="./node()"/>
              </xsl:for-each>
            </xsl:when>
            
            <xsl:otherwise>
              <xsl:message terminate="yes">
                <xsl:text>Do not find any element having '</xsl:text>
                <xsl:value-of select="$root-id"/>
                <xsl:text>' as its ID.</xsl:text>
              </xsl:message>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>

        <xsl:otherwise>
          <!--LEGACY-->
          <xsl:call-template name="bgcolorAttribute"/>
          <xsl:call-template name="textAttribute"/>
          <xsl:call-template name="cssStyles"/>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </xsl:otherwise>
      </xsl:choose>
    </fo:block>
  </xsl:template>

  <!--LEGACY-->
  <xsl:template name="bgcolorAttribute">
    <xsl:if test="exists(@bgcolor)">
      <xsl:attribute name="background-color" select="string(@bgcolor)"/>
    </xsl:if>
  </xsl:template>

  <!--LEGACY-->
  <xsl:template name="textAttribute">
    <xsl:if test="exists(@text)">
      <xsl:attribute name="color" select="string(@text)"/>
    </xsl:if>
  </xsl:template>

  <xsl:attribute-set name="section">
  </xsl:attribute-set>

  <xsl:template match="html:section">
    <fo:block xsl:use-attribute-sets="section">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="nav" use-attribute-sets="display-style">
  </xsl:attribute-set>

  <xsl:template match="html:nav">
    <fo:block xsl:use-attribute-sets="nav">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="article">
  </xsl:attribute-set>

  <xsl:template match="html:article">
    <fo:block xsl:use-attribute-sets="article">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="aside" use-attribute-sets="display-style">
  </xsl:attribute-set>

  <xsl:template match="html:aside">
    <fo:block xsl:use-attribute-sets="aside">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="h1" use-attribute-sets="heading-style">
    <xsl:attribute name="font-size">2em</xsl:attribute>
    <xsl:attribute name="space-before">0.67em</xsl:attribute>
    <xsl:attribute name="space-after">0.67em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:h1">
    <fo:block xsl:use-attribute-sets="h1">
      <!--LEGACY-->
      <xsl:call-template name="alignAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>

      <xsl:call-template name="xfcOutlineLevel"/>

      <!-- Example of how to give a value to {{current-heading}}.
      <fo:marker marker-class-name="current-heading">
        <xsl:value-of select="."/>
      </fo:marker>
      -->

      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!--LEGACY-->
  <xsl:template name="alignAttribute">
    <xsl:if test="@align eq 'center' or 
                  @align eq 'right' or 
                  @align eq 'justify' or 
                  @align eq 'left'">
      <xsl:attribute name="text-align" select="string(@align)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template name="xfcOutlineLevel">
    <!-- In an hgroup, the headings following the first one are subtitles. -->
    <xsl:if test="$foProcessor eq 'XFC' and $set-outline-level eq 'yes'">
      <xsl:variable name="heading"
                    select="if (exists(parent::html:hgroup))
                            then 
                                if (empty(preceding-sibling::*))
                                then parent::html:hgroup
                                else ()
                            else ."/>

      <!-- Ignore headings which are contained in sectioning roots
           other than the body. -->
      <xsl:variable name="sectioningRoot"
        select="if (exists($heading))
                then ($heading/ancestor::*[self::html:body or
                                           self::html:blockquote or
                                           self::html:details or
                                           self::html:dialog or
                                           self::html:fieldset or
                                           self::html:figure or
                                           self::html:td])[last()]
                else ()"/> 
      <xsl:variable name="isHeading"
        select="exists($sectioningRoot/self::html:body) or
                ($root-id ne '' and 
                 exists($sectioningRoot/descendant-or-self::*[@id eq $root-id]))"/>

      <xsl:if test="$isHeading">
        <!-- This a gross simplification as the type of heading being used
             (h1, h2, ..., h6) does not always specify the outline level. -->
        <xsl:attribute name="xfc:outline-level" 
                       select="substring-after(local-name(), 'h')"/>
      </xsl:if>
    </xsl:if>
  </xsl:template>

  <xsl:attribute-set name="h2" use-attribute-sets="heading-style">
    <xsl:attribute name="font-size">1.66em</xsl:attribute>
    <xsl:attribute name="space-before">0.75em</xsl:attribute>
    <xsl:attribute name="space-after">0.75em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:h2">
    <fo:block xsl:use-attribute-sets="h2">
      <!--LEGACY-->
      <xsl:call-template name="alignAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>

      <xsl:call-template name="xfcOutlineLevel"/>

      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="h3" use-attribute-sets="heading-style">
    <xsl:attribute name="font-size">1.5em</xsl:attribute>
    <xsl:attribute name="space-before">0.83em</xsl:attribute>
    <xsl:attribute name="space-after">0.83em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:h3">
    <fo:block xsl:use-attribute-sets="h3">
      <!--LEGACY-->
      <xsl:call-template name="alignAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>

      <xsl:call-template name="xfcOutlineLevel"/>

      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="h4" use-attribute-sets="heading-style">
    <xsl:attribute name="font-size">1.17em</xsl:attribute>
    <xsl:attribute name="space-before">1em</xsl:attribute>
    <xsl:attribute name="space-after">1em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:h4">
    <fo:block xsl:use-attribute-sets="h4">
      <!--LEGACY-->
      <xsl:call-template name="alignAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>

      <xsl:call-template name="xfcOutlineLevel"/>

      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="h5" use-attribute-sets="heading-style">
    <xsl:attribute name="font-size">1em</xsl:attribute>
    <xsl:attribute name="space-before">1.33em</xsl:attribute>
    <xsl:attribute name="space-after">1.33em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:h5">
    <fo:block xsl:use-attribute-sets="h5">
      <!--LEGACY-->
      <xsl:call-template name="alignAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>

      <xsl:call-template name="xfcOutlineLevel"/>

      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="h6" use-attribute-sets="heading-style">
    <xsl:attribute name="font-size">0.83em</xsl:attribute>
    <xsl:attribute name="space-before">1.67em</xsl:attribute>
    <xsl:attribute name="space-after">1.67em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:h6">
    <fo:block xsl:use-attribute-sets="h6">
      <!--LEGACY-->
      <xsl:call-template name="alignAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>

      <xsl:call-template name="xfcOutlineLevel"/>

      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="hgroup">
  </xsl:attribute-set>

  <xsl:template match="html:hgroup">
    <fo:block xsl:use-attribute-sets="hgroup">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="header">
  </xsl:attribute-set>

  <xsl:template match="html:header">
    <fo:block xsl:use-attribute-sets="header">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="footer">
  </xsl:attribute-set>

  <xsl:template match="html:footer">
    <fo:block xsl:use-attribute-sets="footer">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="address">
    <xsl:attribute name="font-style">italic</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:address">
    <fo:block xsl:use-attribute-sets="address">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- Grouping content ================================================== -->

  <!-- p ========== -->

  <xsl:attribute-set name="p" use-attribute-sets="block-style">
  </xsl:attribute-set>

  <xsl:template match="html:p">
    <fo:block xsl:use-attribute-sets="p">
      <!--LEGACY-->
      <xsl:call-template name="alignAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:if test="$foProcessor eq 'XEP' and
                    parent::html:li and
                    empty(following-sibling::*)">
        <!-- Minimal workaround for implementation limit in XEP:
             space-after.conditionality="discard" is not implemented,
             fallback value is "retain".
             space-after each paragraph is stacked instead of being 
             overlapped. As a result, there is much too space between 
             list items. 
             There is no such problem with FOP or XFC. -->
        <xsl:attribute name="space-after">0em</xsl:attribute>
      </xsl:if>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>
      
  <!-- hr ========== -->

  <xsl:attribute-set name="hr" use-attribute-sets="semi-compact-block-style">
    <xsl:attribute name="border">0.5pt solid gray</xsl:attribute>
  </xsl:attribute-set>

  <!-- Workaround a limitation of XFC. -->
  <xsl:attribute-set name="hr-xfc" 
                     use-attribute-sets="semi-compact-block-style">
    <xsl:attribute name="background-color">gray</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:hr">
    <xsl:choose>
      <xsl:when test="$foProcessor eq 'XFC'">
        <fo:block xsl:use-attribute-sets="hr-xfc">
          <xsl:call-template name="cssStyles"/>
          <!-- Use this font-size to force the height of the hr. -->
          <xsl:attribute name="font-size" select="'1pt'"/>

          <xsl:call-template name="commonAttributes"/>
          <!-- Workaround a limitation of XFC. -->
          <xsl:text>&#xA0;</xsl:text>
        </fo:block>
      </xsl:when>
      <xsl:otherwise>
        <fo:block xsl:use-attribute-sets="hr">
          <xsl:call-template name="cssStyles"/>

          <xsl:call-template name="commonAttributes"/>
        </fo:block>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- pre ========== -->

  <xsl:attribute-set name="pre" 
                     use-attribute-sets="block-style monospace-style">
    <xsl:attribute name="white-space">pre</xsl:attribute>
    <xsl:attribute name="text-align">left</xsl:attribute>
    <xsl:attribute name="hyphenate">false</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:pre">
    <fo:block xsl:use-attribute-sets="pre">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- blockquote ========== -->

  <xsl:attribute-set name="blockquote" use-attribute-sets="block-style">
    <xsl:attribute name="margin-left">4em</xsl:attribute>
    <xsl:attribute name="margin-right">4em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:blockquote">
    <fo:block xsl:use-attribute-sets="blockquote">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <!-- @cite is not rendered. -->
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- ol ========== -->

  <xsl:attribute-set name="ol" use-attribute-sets="block-style">
    <xsl:attribute
        name="provisional-distance-between-starts">3em</xsl:attribute>
    <xsl:attribute name="provisional-label-separation">0.5em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:ol">
    <fo:list-block xsl:use-attribute-sets="ol">
      <xsl:call-template name="xfcLabelFormat"/>

      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'list-block'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:list-block>
  </xsl:template>

  <xsl:attribute-set name="nested-ol" 
                     use-attribute-sets="ol compact-block-style">
  </xsl:attribute-set>

  <xsl:template match="html:li//html:ol">
    <fo:list-block xsl:use-attribute-sets="nested-ol">
      <xsl:call-template name="xfcLabelFormat"/>

      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'list-block'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:list-block>
  </xsl:template>

  <xsl:template name="xfcLabelFormat">
    <xsl:if test="$foProcessor eq 'XFC'">
      <xsl:variable name="listType" select="u:listStyleType(.)"/>

      <xsl:choose>
        <xsl:when test="self::html:ol">
          <xsl:variable name="olType">
            <xsl:choose>
              <xsl:when test="$listType eq 'a'">lower-alpha</xsl:when>
              <xsl:when test="$listType eq 'A'">upper-alpha</xsl:when>
              <xsl:when test="$listType eq 'i'">lower-roman</xsl:when>
              <xsl:when test="$listType eq 'I'">upper-roman</xsl:when>
              <xsl:otherwise>decimal</xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="start">
            <xsl:if test="number(@start) ge 0">
              <xsl:value-of select="concat(';start=', @start)"/>
            </xsl:if>
          </xsl:variable>

          <xsl:attribute name="xfc:label-format" 
                         select="concat('%{', $olType, $start, '}.')"/>
        </xsl:when>

        <xsl:otherwise>
          <!-- list-style-image not supported by XFC. -->
          <xsl:variable name="bullet">
            <xsl:choose>
              <xsl:when test="$listType eq 'disc'">&#x2022;</xsl:when>
              <xsl:when test="$listType eq 'circle'">&#x274D;</xsl:when>
              <xsl:when test="$listType eq 'square'">&#x25A0;</xsl:when>
              <xsl:when test="$listType eq 'none'">&#xA0;</xsl:when>
              <xsl:otherwise>
                <xsl:variable name="nesting" 
                              select="if (self::html:dir) 
                                      then count(ancestor::html:dir) 
                                      else 
                                          if (self::html:menu) 
                                          then count(ancestor::html:menu) 
                                          else count(ancestor::html:ul)"/>

                <xsl:value-of 
                  select="$ulLiBullets[1 + ($nesting mod count($ulLiBullets))]"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:attribute name="xfc:label-format" select="$bullet"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <!-- ul ========== -->

  <xsl:attribute-set name="ul" use-attribute-sets="block-style">
    <xsl:attribute
        name="provisional-distance-between-starts">2em</xsl:attribute>
    <xsl:attribute name="provisional-label-separation">0.5em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:ul | 
                       html:menu[exists(./html:li)] | 
                       html:dir"> <!--LEGACY-->
    <fo:list-block xsl:use-attribute-sets="ul">
      <xsl:call-template name="xfcLabelFormat"/>

      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'list-block'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:list-block>
  </xsl:template>

  <xsl:attribute-set name="nested-ul" 
                     use-attribute-sets="ul compact-block-style">
  </xsl:attribute-set>

  <xsl:template match="html:li//html:ul">
    <fo:list-block xsl:use-attribute-sets="nested-ul">
      <xsl:call-template name="xfcLabelFormat"/>

      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'list-block'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:list-block>
  </xsl:template>

  <!-- li ========== -->

  <xsl:attribute-set name="li">
    <xsl:attribute name="relative-align">baseline</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="ol-li" use-attribute-sets="li">
  </xsl:attribute-set>

  <!-- ol/li === -->

  <xsl:template match="html:ol/html:li">
    <fo:list-item xsl:use-attribute-sets="ol-li">
      <xsl:call-template name="orderedListItem" />
    </fo:list-item>
  </xsl:template>

  <xsl:attribute-set name="ol-li-number">
  </xsl:attribute-set>

  <xsl:template name="orderedListItem">
    <xsl:call-template name="parentCompactAttribute"/>

    <xsl:call-template name="cssStyles">
      <xsl:with-param name="select" select="'list-item'"/>
    </xsl:call-template>

    <xsl:call-template name="commonAttributes"/>

    <!-- list-style-image not supported for ol. -->
    <xsl:variable name="listType" select="u:listStyleType(.)"/>

    <xsl:variable name="format">
      <xsl:choose>
        <xsl:when test="$listType eq 'a' or
                        $listType eq 'A' or 
                        $listType eq 'i' or
                        $listType eq 'I'">
          <xsl:value-of select="concat($listType, '.')"/>
        </xsl:when>
        <xsl:otherwise>1.</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <fo:list-item-label end-indent="label-end()">
      <fo:block text-align="end">
        <fo:inline xsl:use-attribute-sets="ol-li-number">
          <xsl:call-template name="orderedListItemNumber">
            <xsl:with-param name="format" select="$format"/>
          </xsl:call-template>
        </fo:inline>
      </fo:block>
    </fo:list-item-label>

    <fo:list-item-body start-indent="body-start()">
      <fo:block>
        <xsl:apply-templates/>
      </fo:block>
    </fo:list-item-body>
  </xsl:template>

  <xsl:template name="parentCompactAttribute">
    <!-- No effect unless the user specifies custom attribute sets
         giving vertical margins to li, dt, dd. -->
    <xsl:if test="exists(../@compact)">
      <xsl:attribute name="space-before">0em</xsl:attribute>
      <xsl:attribute name="space-after">0em</xsl:attribute>
    </xsl:if>
  </xsl:template>

  <xsl:template name="orderedListItemNumber">
    <xsl:param name="format">1.</xsl:param>

    <xsl:variable name="value">
      <xsl:choose>
        <xsl:when test="exists(@value) and u:isNumber(@value)">
          <!-- May be negative of null. -->
          <xsl:value-of select="number(@value)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:variable name="anchor" 
            select="(preceding-sibling::html:li[exists(@value) and 
                                                u:isNumber(@value)])[last()]"/>

          <xsl:variable name="anchorPos">
            <xsl:choose>
              <xsl:when test="exists($anchor)">
                <xsl:sequence select="u:indexOfNode(../html:li, $anchor)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:sequence select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="anchorValue">
            <xsl:choose>
              <xsl:when test="exists($anchor)">
                <xsl:sequence select="number($anchor/@value)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:choose>
                  <xsl:when test="exists(../@start) and u:isNumber(../@start)">
                    <xsl:sequence select="number(../@start)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="exists(../@reversed)">
                        <xsl:sequence select="count(../html:li)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:sequence select="1"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>

          <xsl:variable name="pos" select="u:indexOfNode(../html:li, .)"/>

          <xsl:choose>
            <xsl:when test="exists(../@reversed)">
              <xsl:value-of select="$anchorValue - ($pos - $anchorPos)" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$anchorValue + ($pos - $anchorPos)" />
            </xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="number($value) ge 0">
        <!-- The value must be an integer greater than or equal to 0. -->
        <xsl:number value="$value" format="{$format}"/>
      </xsl:when>
      <xsl:otherwise>
        <!-- Fallback -->
        <xsl:value-of select="concat($value, '.')" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- ul/li === -->

  <xsl:attribute-set name="ul-li" use-attribute-sets="li">
  </xsl:attribute-set>

  <xsl:template match="html:ul/html:li |
                       html:menu/html:li |
                       html:dir/html:li"> <!--LEGACY-->
    <fo:list-item xsl:use-attribute-sets="ul-li">
      <xsl:call-template name="listItem"/>
    </fo:list-item>
  </xsl:template>

  <xsl:attribute-set name="ul-li-image">
  </xsl:attribute-set>

  <xsl:attribute-set name="ul-li-bullet">
  </xsl:attribute-set>

  <xsl:template name="listItem">
    <xsl:call-template name="parentCompactAttribute"/>

    <xsl:call-template name="cssStyles">
      <xsl:with-param name="select" select="'list-item'"/>
    </xsl:call-template>

    <!-- list-style-image not supported by XFC. -->
    <xsl:variable name="listImage" select="u:listStyleImage(.)"/>
    <xsl:variable name="listType" select="u:listStyleType(.)"/>

    <xsl:variable name="image">
      <xsl:if test="starts-with($listImage, 'url(') and
                    $foProcessor ne 'XFC'">
        <xsl:value-of select="$listImage"/>
      </xsl:if>
    </xsl:variable>

    <xsl:variable name="bullet">
      <xsl:if test="not(starts-with($listImage, 'url(')) or 
                    $foProcessor eq 'XFC'">
        <xsl:choose>
          <xsl:when test="$listType eq 'disc'">&#x2022;</xsl:when>
          <xsl:when test="$listType eq 'circle'">&#x274D;</xsl:when>
          <xsl:when test="$listType eq 'square'">&#x25A0;</xsl:when>
          <xsl:when test="$listType eq 'none'">&#xA0;</xsl:when>
          <xsl:otherwise>
            <xsl:variable name="nesting" 
              select="if (parent::html:dir) 
                      then (count(ancestor::html:dir) - 1) 
                      else
                          if (parent::html:menu) 
                          then (count(ancestor::html:menu) - 1) 
                          else (count(ancestor::html:ul) - 1)"/>
            <xsl:value-of 
              select="$ulLiBullets[1 + ($nesting mod count($ulLiBullets))]"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>
    </xsl:variable>

    <xsl:call-template name="commonAttributes"/>

    <fo:list-item-label end-indent="label-end()">
      <fo:block text-align="end">
        <xsl:choose>
          <!-- list-style-image supersedes list-style-type. -->
          <xsl:when test="$image ne ''">
            <fo:external-graphic src="{$image}" display-align="center"
                                 xsl:use-attribute-sets="ul-li-image"/>
          </xsl:when>
          <xsl:otherwise>
            <fo:inline xsl:use-attribute-sets="ul-li-bullet">
              <xsl:if test="$bullet eq '&#x25A0;' or $bullet eq '&#x274D;'">
                <xsl:if test="$foProcessor ne 'XFC'">
                  <xsl:attribute name="font-family">ZapfDingbats</xsl:attribute>
                </xsl:if>
                <xsl:attribute name="font-size">0.5em</xsl:attribute>
                <xsl:attribute name="vertical-align">middle</xsl:attribute>
              </xsl:if>

              <xsl:value-of select="$bullet"/>
            </fo:inline>
          </xsl:otherwise>
        </xsl:choose>
      </fo:block>
    </fo:list-item-label>

    <fo:list-item-body start-indent="body-start()">
      <fo:block>
        <xsl:apply-templates/>
      </fo:block>
    </fo:list-item-body>
  </xsl:template>

  <!-- dl ========== -->

  <xsl:attribute-set name="dl" use-attribute-sets="block-style">
  </xsl:attribute-set>

  <xsl:template match="html:dl">
    <fo:block xsl:use-attribute-sets="dl">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- dt ========== -->

  <xsl:attribute-set name="dt">
    <xsl:attribute name="keep-with-next.within-column">always</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:dt">
    <fo:block xsl:use-attribute-sets="dt">
      <xsl:call-template name="parentCompactAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- dd ========== -->

  <xsl:attribute-set name="dd">
    <xsl:attribute name="margin-left">2em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:dd">
    <fo:block xsl:use-attribute-sets="dd">
      <xsl:call-template name="parentCompactAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- figure ========== -->

  <xsl:attribute-set name="figure" use-attribute-sets="block-style">
    <xsl:attribute name="margin-left">4em</xsl:attribute>
    <xsl:attribute name="margin-right">4em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:figure">
    <fo:block xsl:use-attribute-sets="figure">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- figcaption ========== -->

  <xsl:attribute-set name="figcaption">
    <xsl:attribute name="text-align">left</xsl:attribute>
    <xsl:attribute name="hyphenate">false</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:figcaption">
    <fo:block xsl:use-attribute-sets="figcaption">
      <xsl:call-template name="cssStyles"/>

      <xsl:choose>
        <xsl:when test="exists(preceding-sibling::*)">
          <xsl:attribute 
            name="keep-with-previous.within-column">always</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute
            name="keep-with-next.within-column">always</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- div ========== -->

  <xsl:attribute-set name="div">
  </xsl:attribute-set>

  <xsl:template match="html:div">
    <fo:block xsl:use-attribute-sets="div">
      <!--LEGACY-->
      <xsl:call-template name="alignAttribute"/>
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- center ========== -->
  <!--LEGACY-->

  <xsl:attribute-set name="center">
    <xsl:attribute name="text-align">center</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:center">
    <fo:block xsl:use-attribute-sets="center">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- Text-level semantics ============================================== -->

  <!-- em ========== -->

  <xsl:attribute-set name="em">
    <xsl:attribute name="font-style">italic</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:em">
    <fo:inline xsl:use-attribute-sets="em">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- strong ========== -->

  <xsl:attribute-set name="strong">
    <xsl:attribute name="font-weight">bold</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:strong">
    <fo:inline xsl:use-attribute-sets="strong">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- small ========== -->

  <xsl:attribute-set name="small">
    <xsl:attribute name="font-size">smaller</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:small">
    <fo:inline xsl:use-attribute-sets="small">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- big ========== -->
  <!--LEGACY-->

  <xsl:attribute-set name="big">
    <xsl:attribute name="font-size">larger</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:big">
    <fo:inline xsl:use-attribute-sets="big">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- s ========== -->

  <xsl:attribute-set name="s">
    <xsl:attribute name="text-decoration">line-through</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:s">
    <fo:inline xsl:use-attribute-sets="s">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- strike ========== -->
  <!--LEGACY-->

  <xsl:attribute-set name="strike">
    <xsl:attribute name="text-decoration">line-through</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:strike">
    <fo:inline xsl:use-attribute-sets="strike">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- cite ========== -->

  <xsl:attribute-set name="cite">
    <xsl:attribute name="font-style">italic</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:cite">
    <fo:inline xsl:use-attribute-sets="cite">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- q ========== -->

  <xsl:attribute-set name="q">
  </xsl:attribute-set>

  <xsl:attribute-set name="q-quote">
  </xsl:attribute-set>

  <xsl:template match="html:q">
    <fo:inline xsl:use-attribute-sets="q">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>

      <xsl:variable name="lang"
        select="lower-case(if (exists(ancestor-or-self::*/@*:lang))
                           then string((ancestor-or-self::*/@*:lang)[last()])
                           else 'en')"/>
    
      <xsl:variable name="nesting" select="count(ancestor::html:q)"/>

      <fo:inline xsl:use-attribute-sets="q-quote">
        <xsl:value-of select="u:quote($lang, $nesting, false())"/>
      </fo:inline>

      <xsl:apply-templates/>

      <fo:inline xsl:use-attribute-sets="q-quote">
        <xsl:value-of select="u:quote($lang, $nesting, true())"/>
      </fo:inline>
    </fo:inline>
  </xsl:template>
  
  <xsl:param name="knownQuotes">
    <!-- Default -->

    <u:entry xml:lang="??">
      <u:quote>"</u:quote><u:quote>"</u:quote>
      <u:quote>'</u:quote><u:quote>'</u:quote>
    </u:entry>

    <!-- Switzerland: << >> < > -->

    <u:entry xml:lang="de-ch">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2039;</u:quote><u:quote>&#x203A;</u:quote>
    </u:entry>
    <u:entry xml:lang="fr-ch">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2039;</u:quote><u:quote>&#x203A;</u:quote>
    </u:entry>
    <u:entry xml:lang="it-ch">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2039;</u:quote><u:quote>&#x203A;</u:quote>
    </u:entry>

    <!-- Afrikaans, Dutch, and Polish: ,, '' , ' -->

    <u:entry xml:lang="af">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201D;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>
    <u:entry xml:lang="nl">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201D;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>
    <u:entry xml:lang="pl">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201D;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>

    <!-- Bulgarian, Czech, German, Icelandic, Lithuanian,  -->
    <!-- Slovak, Serbian, Romanian: ,, `` , ` -->

    <u:entry xml:lang="bg">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201C;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2018;</u:quote>
    </u:entry>
    <u:entry xml:lang="cs">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201C;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2018;</u:quote>
    </u:entry>
    <u:entry xml:lang="de">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201C;</u:quote>
    <u:quote>&#x201A;</u:quote><u:quote>&#x2018;</u:quote>
    </u:entry>
    <u:entry xml:lang="is">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201C;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2018;</u:quote>
    </u:entry>
    <u:entry xml:lang="lt">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201C;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2018;</u:quote>
    </u:entry>
    <u:entry xml:lang="sk">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201C;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2018;</u:quote>
    </u:entry>
    <u:entry xml:lang="sr">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201C;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2018;</u:quote>
    </u:entry>
    <u:entry xml:lang="ro">
      <u:quote>&#x201E;</u:quote><u:quote>&#x201C;</u:quote>
      <u:quote>&#x201A;</u:quote><u:quote>&#x2018;</u:quote>
    </u:entry>

    <!-- Danish, and Croatian: >> << > < -->

    <u:entry xml:lang="da">
      <u:quote>&#x00BB;</u:quote><u:quote>&#x00AB;</u:quote>
    <u:quote>&#x203A;</u:quote><u:quote>&#x2039;</u:quote>
    </u:entry>
    <u:entry xml:lang="hr">
      <u:quote>&#x00BB;</u:quote><u:quote>&#x00AB;</u:quote>
      <u:quote>&#x203A;</u:quote><u:quote>&#x2039;</u:quote>
    </u:entry>

    <!-- Greek, Spannish, Albanian, and Turkish: << >> < > -->

    <u:entry xml:lang="el">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2039;</u:quote><u:quote>&#x203A;</u:quote>
    </u:entry>
    <u:entry xml:lang="es">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2039;</u:quote><u:quote>&#x203A;</u:quote>
    </u:entry>
    <u:entry xml:lang="sq">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2039;</u:quote><u:quote>&#x203A;</u:quote>
    </u:entry>
    <u:entry xml:lang="tr">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2039;</u:quote><u:quote>&#x203A;</u:quote>
    </u:entry>

    <!-- French: <<_ _>> `` '' -->

    <u:entry xml:lang="fr">
      <u:quote>&#x00AB;&#x2005;</u:quote><u:quote>&#x2005;&#x00BB;</u:quote>
      <u:quote>&#x201C;</u:quote><u:quote>&#x201D;</u:quote>
    </u:entry>

    <!-- British English: ` ' `` '' -->

    <u:entry xml:lang="en-gb">
      <u:quote>&#x2018;</u:quote><u:quote>&#x2019;</u:quote>
      <u:quote>&#x201C;</u:quote><u:quote>&#x201D;</u:quote>
    </u:entry>

    <!-- American English, Irish, and Portuguese: `` '' ` ' -->

    <u:entry xml:lang="en">
      <u:quote>&#x201C;</u:quote><u:quote>&#x201D;</u:quote>
      <u:quote>&#x2018;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>
    <u:entry xml:lang="ga">
      <u:quote>&#x201C;</u:quote><u:quote>&#x201D;</u:quote>
      <u:quote>&#x2018;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>
    <u:entry xml:lang="pt">
      <u:quote>&#x201C;</u:quote><u:quote>&#x201D;</u:quote>
      <u:quote>&#x2018;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>

    <!-- Finnish, and Swedish: '' '' ' ' -->

    <u:entry xml:lang="fi">
      <u:quote>&#x201D;</u:quote><u:quote>&#x201D;</u:quote>
      <u:quote>&#x2019;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>
    <u:entry xml:lang="sv">
      <u:quote>&#x201D;</u:quote><u:quote>&#x201D;</u:quote>
      <u:quote>&#x2019;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>

    <!-- Japanese, and Chinese: |~ _| ||~ _|| -->

    <u:entry xml:lang="ja">
      <u:quote>&#x300C;</u:quote><u:quote>&#x300D;</u:quote>
      <u:quote>&#x300E;</u:quote><u:quote>&#x300F;</u:quote>
    </u:entry>
    <u:entry xml:lang="zh">
      <u:quote>&#x300C;</u:quote><u:quote>&#x300D;</u:quote>
      <u:quote>&#x300E;</u:quote><u:quote>&#x300F;</u:quote>
    </u:entry>

    <!-- Norwegian Bokmal, Norwegian, Norwegian Nynorsk: << >> ` ' -->

    <u:entry xml:lang="nb">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2018;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>
    <u:entry xml:lang="no">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2018;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>
    <u:entry xml:lang="nn">
      <u:quote>&#x00AB;</u:quote><u:quote>&#x00BB;</u:quote>
      <u:quote>&#x2018;</u:quote><u:quote>&#x2019;</u:quote>
    </u:entry>
  </xsl:param>

  <xsl:function name="u:quote" as="xs:string">
    <xsl:param name="lang" as="xs:string"/>
    <xsl:param name="nesting" as="xs:integer"/>
    <xsl:param name="closing" as="xs:boolean"/>

    <xsl:variable name="entries" as="element()*">
      <xsl:for-each select="$knownQuotes/u:entry">
        <xsl:sequence select="if (starts-with($lang, string(./@xml:lang)))
                              then .
                              else ()"/>
      </xsl:for-each>
    </xsl:variable>

    <xsl:variable name="entry" 
                  select="if (count($entries) gt 0)
                          then ($entries)[1]
                          else ($knownQuotes/u:entry)[1]" 
                  as="element()"/>

    <xsl:variable name="index" 
      select="1 + (2*($nesting mod 2)) + (if ($closing) then 1 else 0)"
      as="xs:integer"/>

    <xsl:sequence select="string($entry/u:quote[$index])"/>
  </xsl:function>

  <!-- dfn ========== -->

  <xsl:attribute-set name="dfn">
  </xsl:attribute-set>

  <xsl:template match="html:dfn">
    <fo:inline xsl:use-attribute-sets="dfn">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- abbr ========== -->

  <xsl:attribute-set name="abbr">
  </xsl:attribute-set>

  <xsl:template match="html:abbr">
    <fo:inline xsl:use-attribute-sets="abbr">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- acronym ========== -->
  <!--LEGACY-->

  <xsl:attribute-set name="acronym">
  </xsl:attribute-set>

  <xsl:template match="html:acronym">
    <fo:inline xsl:use-attribute-sets="acronym">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- time ========== -->

  <xsl:attribute-set name="time">
  </xsl:attribute-set>

  <xsl:template match="html:time">
    <fo:inline xsl:use-attribute-sets="time">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- code ========== -->

  <xsl:attribute-set name="code" use-attribute-sets="monospace-style">
  </xsl:attribute-set>

  <xsl:template match="html:code">
    <fo:inline xsl:use-attribute-sets="code">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- var ========== -->

  <xsl:attribute-set name="var">
    <xsl:attribute name="font-style">italic</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:var">
    <fo:inline xsl:use-attribute-sets="var">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- samp ========== -->

  <xsl:attribute-set name="samp" use-attribute-sets="monospace-style">
  </xsl:attribute-set>

  <xsl:template match="html:samp">
    <fo:inline xsl:use-attribute-sets="samp">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- kbd ========== -->

  <xsl:attribute-set name="kbd" use-attribute-sets="monospace-style">
  </xsl:attribute-set>

  <xsl:template match="html:kbd">
    <fo:inline xsl:use-attribute-sets="kbd">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- tt ========== -->
  <!--LEGACY-->

  <xsl:attribute-set name="tt" use-attribute-sets="monospace-style">
  </xsl:attribute-set>

  <xsl:template match="html:tt">
    <fo:inline xsl:use-attribute-sets="tt">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- sub ========== -->

  <xsl:attribute-set name="sub">
    <xsl:attribute name="baseline-shift">sub</xsl:attribute>
    <xsl:attribute name="font-size">smaller</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:sub">
    <fo:inline xsl:use-attribute-sets="sub">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- sup ========== -->

  <xsl:attribute-set name="sup">
    <xsl:attribute name="baseline-shift">super</xsl:attribute>
    <xsl:attribute name="font-size">smaller</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:sup">
    <fo:inline xsl:use-attribute-sets="sup">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- i ========== -->

  <xsl:attribute-set name="i">
    <xsl:attribute name="font-style">italic</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:i">
    <fo:inline xsl:use-attribute-sets="i">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- b ========== -->

  <xsl:attribute-set name="b">
    <xsl:attribute name="font-weight">bold</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:b">
    <fo:inline xsl:use-attribute-sets="b">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- u ========== -->

  <xsl:attribute-set name="u">
    <xsl:attribute name="text-decoration">underline</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:u">
    <fo:inline xsl:use-attribute-sets="u">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- mark ========== -->

  <xsl:attribute-set name="mark">
    <xsl:attribute name="background-color">yellow</xsl:attribute>
    <xsl:attribute name="color">black</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:mark">
    <fo:inline xsl:use-attribute-sets="mark">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- basefont ========== -->

  <xsl:template match="html:basefont"/>

  <!-- font ========== -->

  <xsl:attribute-set name="font">
  </xsl:attribute-set>

  <xsl:template match="html:font">
    <fo:inline xsl:use-attribute-sets="font">
      <xsl:if test="exists(@face)">
        <xsl:attribute name="font-family" select="string(@face)"/>
      </xsl:if>

      <xsl:if test="exists(@color)">
        <xsl:attribute name="color" select="string(@color)"/>
      </xsl:if>

      <xsl:if test="exists(@size)">
        <xsl:attribute name="font-size">
          <xsl:choose>
            <xsl:when test="@size eq '1'">x-small</xsl:when>
            <xsl:when test="@size eq '2'">small</xsl:when>
            <xsl:when test="@size eq '3'">medium</xsl:when>
            <xsl:when test="@size eq '4'">large</xsl:when>
            <xsl:when test="@size eq '5'">x-large</xsl:when>
            <xsl:when test="@size eq '6'">xx-large;</xsl:when>
            <xsl:when test="@size eq '7'">xx-large;</xsl:when>
            <xsl:when test="@size eq '-3'">.67em</xsl:when>
            <xsl:when test="@size eq '-2'">.75em</xsl:when>
            <xsl:when test="@size eq '-1'">.83em</xsl:when>
            <xsl:when test="@size eq '+1'">1.17em</xsl:when>
            <xsl:when test="@size eq '+2'">1.5em</xsl:when>
            <xsl:when test="@size eq '+3'">2em</xsl:when>
            <xsl:otherwise>1em</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </xsl:if>

      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- ruby, rt, rp ========== -->

  <!-- XEP, XFC, FOP do not support fo:inline-container
       which is needed to correctly render ruby. -->

  <xsl:attribute-set name="ruby">
    <xsl:attribute name="alignment-baseline">after-edge</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="ruby-inline">
  </xsl:attribute-set>

  <xsl:template match="html:ruby">
    <xsl:choose>
      <xsl:when test="$foProcessor ne 'AHF'">
        <fo:inline xsl:use-attribute-sets="ruby-inline">
          <xsl:call-template name="cssStyles">
            <xsl:with-param name="select" select="'inline'"/>
          </xsl:call-template>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:inline>
      </xsl:when>

      <xsl:otherwise>
        <xsl:variable name="bottomRows" as="element()*">
          <xsl:for-each-group select="./node()" 
            group-adjacent="if (self::html:rt or self::html:rp)
                            then 'top'
                            else 'bottom'">
            <xsl:choose>
              <xsl:when test="current-grouping-key() eq 'bottom'">
                <fo:table-row>
                  <fo:table-cell>
                    <fo:block>
                      <xsl:apply-templates select="current-group()"/>
                    </fo:block>
                  </fo:table-cell>
                </fo:table-row>
              </xsl:when>
              <xsl:otherwise>
                <xsl:sequence select="()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each-group>
        </xsl:variable>

        <xsl:variable name="topRows" as="element()*">
          <xsl:for-each-group select="./node()" 
            group-adjacent="if (self::html:rt or self::html:rp)
                            then 'top'
                            else 'bottom'">
            <xsl:choose>
              <xsl:when test="current-grouping-key() eq 'top'">
                <fo:table-row>
                  <fo:table-cell>
                    <fo:block text-align="center">
                      <xsl:apply-templates select="current-group()"/>
                    </fo:block>
                  </fo:table-cell>
                </fo:table-row>
              </xsl:when>
              <xsl:otherwise>
                <xsl:sequence select="()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:for-each-group>
        </xsl:variable>

        <xsl:variable name="emptyRow" as="element()">
          <fo:table-row>
            <fo:table-cell>
              <fo:block>&#xA0;</fo:block>
            </fo:table-cell>
          </fo:table-row>
        </xsl:variable>

        <xsl:variable name="ruby" select="."/>

        <xsl:for-each select="$bottomRows">
          <xsl:variable name="index" select="position()"/>

          <fo:inline-container xsl:use-attribute-sets="ruby">
            <xsl:for-each select="$ruby">
              <xsl:call-template name="cssStyles">
                <xsl:with-param name="select" select="'inline-container'"/>
              </xsl:call-template>

              <xsl:if test="$index gt 1">
                <xsl:call-template name="idAttribute"/>
              </xsl:if>
              <xsl:call-template name="localizationAttributes"/>
              <xsl:call-template name="xmlSpaceAttribute"/>
            </xsl:for-each>

            <fo:table>
              <fo:table-body>
                <xsl:sequence select="if ($index le count($topRows))
                                      then $topRows[$index]
                                      else $emptyRow"/>

                <xsl:sequence select="."/>
              </fo:table-body>
            </fo:table>
          </fo:inline-container>
        </xsl:for-each>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:attribute-set name="rt">
    <xsl:attribute name="font-size">smaller</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="rt-inline">
    <xsl:attribute name="color">#800080</xsl:attribute>
    <xsl:attribute name="font-size">smaller</xsl:attribute>
    <xsl:attribute name="space-start">0.125em</xsl:attribute>
    <xsl:attribute name="space-end">0.125em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:rt">
    <xsl:choose>
      <xsl:when test="$foProcessor ne 'AHF'">
        <fo:inline xsl:use-attribute-sets="rt-inline">
          <xsl:call-template name="cssStyles">
            <xsl:with-param name="select" select="'inline'"/>
          </xsl:call-template>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:inline>
      </xsl:when>

      <xsl:otherwise>
        <fo:inline xsl:use-attribute-sets="rt">
          <xsl:call-template name="cssStyles">
            <xsl:with-param name="select" select="'inline'"/>
          </xsl:call-template>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:inline>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:attribute-set name="rp-inline">
    <xsl:attribute name="color">gray</xsl:attribute>
    <xsl:attribute name="font-size">smaller</xsl:attribute>
    <xsl:attribute name="space-start">0.125em</xsl:attribute>
    <xsl:attribute name="space-end">0.125em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:rp">
    <xsl:if test="$foProcessor ne 'AHF'">
      <fo:inline xsl:use-attribute-sets="rp-inline">
        <xsl:call-template name="cssStyles">
          <xsl:with-param name="select" select="'inline'"/>
        </xsl:call-template>

        <xsl:call-template name="commonAttributes"/>
        <xsl:apply-templates/>
      </fo:inline>
    </xsl:if>
    <!-- Otherwise, not rendered. -->
  </xsl:template>

  <!-- bdi, bdo ========== -->
  <!-- No attribute-set for bdi, bdo -->

  <xsl:template match="html:bdi | html:bdo">
    <xsl:choose>
      <xsl:when test="@dir eq 'ltr' or @dir eq 'rtl'">
        <fo:bidi-override direction="{@dir}" unicode-bidi="bidi-override">
          <xsl:call-template name="cssStyles">
            <xsl:with-param name="select" select="'bidi-override'"/>
          </xsl:call-template>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:bidi-override>
      </xsl:when>
      <xsl:otherwise>
        <fo:inline>
          <xsl:call-template name="cssStyles">
            <xsl:with-param name="select" select="'inline'"/>
          </xsl:call-template>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:inline>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- span ========== -->

  <xsl:attribute-set name="span">
  </xsl:attribute-set>

  <xsl:template match="html:span">
    <fo:inline xsl:use-attribute-sets="span">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- br ========== -->
  <!-- No attribute-set for br -->

  <xsl:template match="html:br">
    <fo:block>
      <xsl:call-template name="commonAttributes"/>
    </fo:block>
  </xsl:template>

  <!-- wbr ========== -->

  <xsl:template match="html:wbr"/>

  <!-- a ========== -->

  <xsl:attribute-set name="a">
  </xsl:attribute-set>

  <xsl:attribute-set name="a-link" use-attribute-sets="link-style">
  </xsl:attribute-set>

  <xsl:attribute-set name="a-block">
  </xsl:attribute-set>

  <xsl:attribute-set name="a-link-block">
    <xsl:attribute name="border">1pt solid navy</xsl:attribute>
    <xsl:attribute name="padding">0.25em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:a">
    <xsl:choose>
      <xsl:when test="u:containsBlock(.)">
        <xsl:choose>
          <xsl:when test="exists(@href)">
            <xsl:choose>
              <xsl:when test="$foProcessor eq 'XFC'">
                <!-- With XFC, fo:basic-link can only contain text 
                     and inline-level objects.-->
                <fo:block xsl:use-attribute-sets="a-link-block">
                  <xsl:call-template name="cssStyles"/>

                  <xsl:call-template name="commonAttributes"/>
                  <xsl:call-template name="nameToId"/>
                  <xsl:apply-templates/>
                </fo:block>
              </xsl:when>
              <xsl:otherwise>
                <fo:basic-link>
                  <xsl:call-template name="linkDestination">
                    <xsl:with-param name="href" select="string(@href)"/>
                  </xsl:call-template>

                  <fo:block xsl:use-attribute-sets="a-link-block">
                    <xsl:call-template name="cssStyles"/>

                    <xsl:call-template name="commonAttributes"/>
                    <xsl:call-template name="nameToId"/>
                    <xsl:apply-templates/>
                  </fo:block>
                </fo:basic-link>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:otherwise>
            <fo:block xsl:use-attribute-sets="a-block">
              <xsl:call-template name="cssStyles"/>

              <xsl:call-template name="commonAttributes"/>
              <xsl:call-template name="nameToId"/>
              <xsl:apply-templates/>
            </fo:block>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>

      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="exists(@href)">
            <fo:basic-link xsl:use-attribute-sets="a-link">
              <xsl:call-template name="linkDestination">
                <xsl:with-param name="href" select="string(@href)"/>
              </xsl:call-template>

              <xsl:call-template name="cssStyles">
                <xsl:with-param name="select" select="'inline'"/>
              </xsl:call-template>

              <xsl:call-template name="commonAttributes"/>
              <xsl:call-template name="nameToId"/>
              <xsl:apply-templates/>

              <xsl:call-template name="showLinkInfo">
                <xsl:with-param name="href" select="string(@href)"/>
              </xsl:call-template>
            </fo:basic-link>
          </xsl:when>
          <xsl:otherwise>
            <fo:inline xsl:use-attribute-sets="a">
              <xsl:call-template name="cssStyles">
                <xsl:with-param name="select" select="'inline'"/>
              </xsl:call-template>

              <xsl:call-template name="commonAttributes"/>
              <xsl:call-template name="nameToId"/>
              <xsl:apply-templates/>
            </fo:inline>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="linkDestination">
    <xsl:param name="href" select="''"/>

    <xsl:choose>
      <xsl:when test="starts-with($href, '#')">
        <xsl:attribute name="internal-destination" 
                       select="substring-after($href, '#')"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="href2">
          <xsl:choose>
            <xsl:when test="$resolve-a-href eq 'yes'">
              <xsl:call-template name="resolveHref">
                <xsl:with-param name="href" select="$href"/>
              </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$href"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>

        <xsl:attribute name="external-destination" 
                       select="concat('url(', $href2, ')')"/>
      </xsl:otherwise>
    </xsl:choose>

    <xsl:if test="exists(@title)">
      <xsl:attribute name="role" select="normalize-space(@title)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template name="nameToId">
    <xsl:if test="exists(@name) and empty(@id)">
      <xsl:attribute name="id" select="string(@name)"/>
    </xsl:if>
  </xsl:template>

  <xsl:attribute-set name="page-ref">
  </xsl:attribute-set>

  <xsl:attribute-set name="external-href">
  </xsl:attribute-set>

  <xsl:template name="showLinkInfo">
    <xsl:param name="href" select="''"/>

    <xsl:choose>
      <xsl:when test="starts-with($href, '#')">
        <xsl:if test="$show-xref-page eq 'yes'">
          <xsl:variable name="id" select="substring-after($href, '#')"/>
          <xsl:if test="$id ne ''">
            <fo:inline xsl:use-attribute-sets="page-ref">
              <xsl:value-of select="$page-ref-before"/>
              <fo:page-number-citation ref-id="{$id}"/>
              <xsl:value-of select="$page-ref-after"/>
            </fo:inline>
          </xsl:if>
        </xsl:if>
      </xsl:when>
      <xsl:otherwise>
        <xsl:if test="$show-external-links eq 'yes'">
          <fo:inline xsl:use-attribute-sets="external-href">
            <xsl:value-of select="concat($external-href-before,
                                         $href, 
                                         $external-href-after)"/>
          </fo:inline>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Edits ============================================================= -->

  <!-- ins ========== -->

  <xsl:attribute-set name="ins">
    <xsl:attribute name="text-decoration">underline</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="ins-block">
    <xsl:attribute name="border-right">1pt solid blue</xsl:attribute>
    <xsl:attribute name="padding-right">0.25em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:ins">
    <xsl:choose>
      <xsl:when test="u:containsBlock(.)">
        <fo:block xsl:use-attribute-sets="ins-block">
          <xsl:call-template name="cssStyles"/>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:block>
      </xsl:when>
      <xsl:otherwise>
        <fo:inline xsl:use-attribute-sets="ins">
          <xsl:call-template name="cssStyles">
            <xsl:with-param name="select" select="'inline'"/>
          </xsl:call-template>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:inline>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- del ========== -->

  <xsl:attribute-set name="del">
    <xsl:attribute name="text-decoration">line-through</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="del-block">
    <xsl:attribute name="color">gray</xsl:attribute>
    <xsl:attribute name="border-right">1pt solid gray</xsl:attribute>
    <xsl:attribute name="padding-right">0.25em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:del">
    <xsl:choose>
      <xsl:when test="u:containsBlock(.)">
        <fo:block xsl:use-attribute-sets="del-block">
          <xsl:call-template name="cssStyles"/>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:block>
      </xsl:when>
      <xsl:otherwise>
        <fo:inline xsl:use-attribute-sets="del">
          <xsl:call-template name="cssStyles">
            <xsl:with-param name="select" select="'inline'"/>
          </xsl:call-template>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:inline>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- Embedded content ================================================== -->

  <!-- img ========== -->

  <xsl:attribute-set name="img">
  </xsl:attribute-set>

  <xsl:template match="html:img">
    <xsl:variable name="cssWidth" select="u:cssStyle(., 'width')"/>
    <xsl:variable name="width" 
                  select="if ($cssWidth ne '' and $cssWidth ne 'inherit') 
                          then $cssWidth 
                          else string(@width)"/>

    <xsl:variable name="cssHeight" select="u:cssStyle(., 'height')"/>
    <xsl:variable name="height" 
                  select="if ($cssHeight ne '' and $cssHeight ne 'inherit') 
                          then $cssHeight 
                          else string(@height)" />

    <xsl:if test="not($width eq '0' and $height eq '0')">
      <fo:external-graphic xsl:use-attribute-sets="img">
        <!--LEGACY-->
        <xsl:call-template name="imgAlignAttribute"/>
        <xsl:call-template name="cssStyles">
          <xsl:with-param name="select" select="'external-graphic'"/>
        </xsl:call-template>

        <xsl:attribute name="src">
          <xsl:text>url(</xsl:text>
          <xsl:choose>
            <xsl:when test="$img-src-path ne '' and
                            not(starts-with(@src, '/')) and
                            not(matches(@src, '^[a-zA-Z][a-zA-Z0-9.+-]*:/'))">
              <xsl:value-of select="concat($img-src-path, @src)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:variable name="src">
                <xsl:choose>
                  <xsl:when test="$resolve-img-src eq 'yes'">
                    <xsl:call-template name="resolveHref">
                      <xsl:with-param name="href" select="string(@src)"/>
                    </xsl:call-template>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="string(@src)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:variable>

              <xsl:value-of select="$src"/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:text>)</xsl:text>
        </xsl:attribute>

        <xsl:if test="exists(@alt)">
          <xsl:attribute name="role" select="normalize-space(@alt)"/>
        </xsl:if>

        <xsl:if test="$width ne ''">
          <xsl:choose>
            <xsl:when test="ends-with($width, '%')">
              <!--LEGACY-->
              <xsl:attribute name="width" select="$width"/>
              <xsl:attribute name="content-width">scale-to-fit</xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="content-width" 
                             select="u:asLength($width)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>

        <xsl:if test="$height ne ''">
          <xsl:choose>
            <xsl:when test="ends-with($height, '%')">
              <!--LEGACY-->
              <xsl:attribute name="height" select="$height"/>
              <xsl:attribute name="content-height">scale-to-fit</xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="content-height" 
                             select="u:asLength($height)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>

        <xsl:if test="$width ne '' and $height ne ''">
          <xsl:attribute name="scaling">non-uniform</xsl:attribute>
        </xsl:if>

        <xsl:call-template name="commonAttributes"/>
      </fo:external-graphic>
    </xsl:if>
  </xsl:template>

  <xsl:template name="imgAlignAttribute">
    <xsl:if test="@align eq 'top' or 
                  @align eq 'bottom' or 
                  @align eq 'middle'">
      <xsl:attribute name="vertical-align" select="string(@align)"/>
    </xsl:if>
  </xsl:template>

  <!-- svg:svg, mml:math ========== -->

  <xsl:template match="svg:svg|mml:math">
    <fo:instream-foreign-object>
      <xsl:copy-of select="."/>
    </fo:instream-foreign-object>
  </xsl:template>

  <!-- iframe, object, embed, etc, ========== -->

  <xsl:template match="html:iframe|
                       html:embed|
                       html:object|
                       html:param|
                       html:video|
                       html:audio|
                       html:source|
                       html:track|
                       html:canvas|
                       html:map|
                       html:area">
    <xsl:call-template name="placeholder"/>
  </xsl:template>

  <xsl:attribute-set name="placeholder">
    <xsl:attribute name="font-family">sans-serif</xsl:attribute>
    <xsl:attribute name="font-size">smaller</xsl:attribute>
    <xsl:attribute name="font-style">normal</xsl:attribute>
    <xsl:attribute name="font-weight">normal</xsl:attribute>
    <xsl:attribute name="color">gray</xsl:attribute>
    <xsl:attribute name="border">0.5pt solid gray</xsl:attribute>
    <xsl:attribute name="padding">0.25em</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template name="placeholder">
    <fo:inline xsl:use-attribute-sets="placeholder">
      <xsl:call-template name="idAttribute"/>
      <xsl:value-of select="local-name()"/>
      <!-- Do not traverse. -->
    </fo:inline>
  </xsl:template>

  <!-- Forms ============================================================= -->

  <!-- form ========== -->

  <xsl:attribute-set name="form" use-attribute-sets="block-style">
  </xsl:attribute-set>

  <xsl:template match="html:form">
    <fo:block xsl:use-attribute-sets="form">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- fieldset ========== -->

  <xsl:attribute-set name="fieldset">
    <xsl:attribute name="border">1pt groove #C0C0C0</xsl:attribute>
    <xsl:attribute name="padding-top">0.25em</xsl:attribute>
    <xsl:attribute name="padding-right">0.25em</xsl:attribute>
    <xsl:attribute name="padding-bottom">0.25em</xsl:attribute>
    <xsl:attribute name="padding-left">0.25em</xsl:attribute>
    <xsl:attribute name="margin">1pt</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="fieldset-with-legend" use-attribute-sets="fieldset">
    <xsl:attribute name="padding-top">0pt</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:fieldset">
    <xsl:choose>
      <xsl:when test="exists(./html:legend)">
        <fo:block xsl:use-attribute-sets="fieldset-with-legend">
          <xsl:call-template name="cssStyles"/>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:block>
      </xsl:when>
      <xsl:otherwise>
        <fo:block xsl:use-attribute-sets="fieldset">
          <xsl:call-template name="cssStyles"/>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:block>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- legend ========== -->

  <xsl:attribute-set name="legend">
    <xsl:attribute name="border-bottom">1pt groove #C0C0C0</xsl:attribute>
    <xsl:attribute name="padding-top">0.25em</xsl:attribute>
    <xsl:attribute name="padding-bottom">0.25em</xsl:attribute>
    <xsl:attribute name="margin-bottom">1pt</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:legend">
    <fo:block xsl:use-attribute-sets="legend">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <!-- label ========== -->

  <xsl:attribute-set name="label">
  </xsl:attribute-set>

  <xsl:template match="html:label">
    <fo:inline xsl:use-attribute-sets="label">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'inline'"/>
      </xsl:call-template>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:inline>
  </xsl:template>

  <!-- input, button, select, etc, ========== -->

  <xsl:template match="html:input|
                       html:button|
                       html:select|
                       html:optgroup|
                       html:option|
                       html:textarea|
                       html:keygen|
                       html:output|
                       html:progress|
                       html:meter">
    <xsl:call-template name="placeholder"/>
  </xsl:template>
  
  <!--NOT RENDERED-->
  <xsl:template match="html:datalist"/>

  <!-- Interactive elements ============================================== -->

  <xsl:attribute-set name="details">
  </xsl:attribute-set>

  <xsl:template match="html:details">
    <fo:block xsl:use-attribute-sets="details">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>

      <xsl:if test="empty(./html:summary)">
        <xsl:call-template name="defaultSummary"/>
      </xsl:if>

      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="summary">
  </xsl:attribute-set>

  <xsl:attribute-set name="default-summary" use-attribute-sets="summary">
  </xsl:attribute-set>

  <xsl:template name="defaultSummary">
    <fo:block xsl:use-attribute-sets="default-summary">
      <xsl:text>Details</xsl:text>
    </fo:block>
  </xsl:template>

  <xsl:template match="html:summary">
    <fo:block xsl:use-attribute-sets="summary">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:attribute-set name="menu" use-attribute-sets="block-style">
  </xsl:attribute-set>

  <xsl:template match="html:menu">
    <fo:block xsl:use-attribute-sets="menu">
      <xsl:call-template name="cssStyles"/>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="html:command |
                       html:dialog">
    <xsl:call-template name="placeholder"/>
  </xsl:template>

</xsl:stylesheet>
