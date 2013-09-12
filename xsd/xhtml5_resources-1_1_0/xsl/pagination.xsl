<?xml version="1.0" encoding="UTF-8"?>

<!-- =====================================================================
     XSLT 2 stylesheets for XHTML 1.0, 1.1, 5.0.
     Page layout. Page header and footer.

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
                xmlns:u="http://www.xmlmind.com/namespace/xmleditor"
                exclude-result-prefixes="xs u"
                version="2.0">

  <!-- layoutMasterSet =================================================== -->

  <xsl:template name="layoutMasterSet">
    <fo:layout-master-set>
      <!-- simple-page-masters ========== -->

      <fo:simple-page-master master-name="first-body-page"
                             page-width="{$page-width}"
                             page-height="{$page-height}"
                             margin-top="{$page-top-margin}"
                             margin-bottom="{$page-bottom-margin}"
                             margin-left="{$page-inner-margin}"
                             margin-right="{$page-outer-margin}">
        <fo:region-body margin-top="{$body-top-margin}"
                        margin-bottom="{$body-bottom-margin}"/>
        <fo:region-before extent="{$header-height}"
                          display-align="before"
                          region-name="first-page-header"/>
        <fo:region-after extent="{$footer-height}"
                         display-align="after"
                         region-name="first-page-footer"/>
      </fo:simple-page-master>

      <fo:simple-page-master master-name="odd-body-page"
                             page-width="{$page-width}"
                             page-height="{$page-height}"
                             margin-top="{$page-top-margin}"
                             margin-bottom="{$page-bottom-margin}"
                             margin-left="{$page-inner-margin}"
                             margin-right="{$page-outer-margin}">
        <fo:region-body margin-top="{$body-top-margin}"
                        margin-bottom="{$body-bottom-margin}"/>
        <fo:region-before extent="{$header-height}"
                          display-align="before"
                          region-name="odd-page-header"/>
        <fo:region-after extent="{$footer-height}"
                          display-align="after"
                          region-name="odd-page-footer"/>
      </fo:simple-page-master>

      <fo:simple-page-master master-name="even-body-page"
                             page-width="{$page-width}"
                             page-height="{$page-height}"
                             margin-top="{$page-top-margin}"
                             margin-bottom="{$page-bottom-margin}"
                             margin-left="{$page-outer-margin}"
                             margin-right="{$page-inner-margin}">
        <fo:region-body margin-top="{$body-top-margin}"
                        margin-bottom="{$body-bottom-margin}"/>
        <fo:region-before extent="{$header-height}"
                          display-align="before"
                          region-name="even-page-header"/>
        <fo:region-after extent="{$footer-height}"
                         display-align="after"
                         region-name="even-page-footer"/>
      </fo:simple-page-master>

      <!-- page-sequence-masters ========== -->

      <fo:page-sequence-master master-name="body">
        <fo:repeatable-page-master-alternatives>
          <fo:conditional-page-master-reference 
            master-reference="first-body-page"
            page-position="first"/>
          <fo:conditional-page-master-reference
            master-reference="odd-body-page"
            odd-or-even="odd"/>
          <fo:conditional-page-master-reference 
            odd-or-even="even" 
            master-reference="even-body-page"/>
        </fo:repeatable-page-master-alternatives>
      </fo:page-sequence-master>

    </fo:layout-master-set>
  </xsl:template>

  <!-- configurePageSequence ============================================= -->

  <xsl:template name="configurePageSequence">
    <xsl:param name="documentTitle" select="''"/>

    <xsl:attribute name="master-reference" select="'body'"/>

    <xsl:attribute name="hyphenate" 
                   select="if ($hyphenate eq 'yes') then 'true' else 'false'"/>

    <xsl:attribute name="initial-page-number">1</xsl:attribute>
    <xsl:attribute name="format">1</xsl:attribute>

    <xsl:call-template name="headersAndFooters">
      <xsl:with-param name="documentTitle" select="$documentTitle"/>
    </xsl:call-template>
  </xsl:template>
  
  <!-- headersAndFooters ================================================= -->

  <xsl:template name="headersAndFooters">
    <xsl:param name="documentTitle" select="''"/>

    <xsl:variable name="currentHeading">
      <fo:retrieve-marker retrieve-class-name="current-heading"/>
    </xsl:variable>
    <xsl:variable name="firstPageHeading" select="()"/>
    <xsl:variable name="evenPageHeading" select="$currentHeading" />
    <xsl:variable name="oddPageHeading" select="$currentHeading" />

    <xsl:variable name="pageNumber">
      <fo:page-number/>
    </xsl:variable>

    <xsl:variable name="pageCount">
      <fo:page-number-citation ref-id="__EOF"/>
    </xsl:variable>

    <xsl:variable name="hl1" 
      select="u:expandHeaderOrFooter($header-left,
                                     $pageNumber, false(), $pageCount, 
                                     $firstPageHeading, $documentTitle)"/>
    <xsl:variable name="hc1" 
      select="u:expandHeaderOrFooter($header-center,
                                     $pageNumber, false(), $pageCount, 
                                     $firstPageHeading, $documentTitle)"/>
    <xsl:variable name="hr1" 
      select="u:expandHeaderOrFooter($header-right,
                                     $pageNumber, false(), $pageCount, 
                                     $firstPageHeading, $documentTitle)"/>

    <xsl:if test="exists($hl1) or exists($hc1) or exists($hr1)">
      <fo:static-content flow-name="first-page-header">
        <xsl:call-template name="tabularHeader">
          <xsl:with-param name="left" select="$hl1"/>
          <xsl:with-param name="center" select="$hc1"/>
          <xsl:with-param name="right" select="$hr1"/>
          <xsl:with-param name="isFooter" select="false()"/>
        </xsl:call-template>
      </fo:static-content>
    </xsl:if>

    <xsl:variable name="hlo" 
      select="u:expandHeaderOrFooter($header-left,
                                     $pageNumber, false(), $pageCount, 
                                     $oddPageHeading, $documentTitle)"/>
    <xsl:variable name="hco" 
      select="u:expandHeaderOrFooter($header-center,
                                     $pageNumber, false(), $pageCount, 
                                     $oddPageHeading, $documentTitle)"/>
    <xsl:variable name="hro" 
      select="u:expandHeaderOrFooter($header-right,
                                     $pageNumber, false(), $pageCount, 
                                     $oddPageHeading, $documentTitle)"/>

    <xsl:if test="exists($hlo) or exists($hco) or exists($hro)">
      <fo:static-content flow-name="odd-page-header">
        <xsl:call-template name="tabularHeader">
          <xsl:with-param name="left" select="$hlo"/>
          <xsl:with-param name="center" select="$hco"/>
          <xsl:with-param name="right" select="$hro"/>
          <xsl:with-param name="isFooter" select="false()"/>
        </xsl:call-template>
      </fo:static-content>
    </xsl:if>

    <xsl:variable name="hle" 
      select="u:expandHeaderOrFooter($header-left,
                                     $pageNumber, true(), $pageCount, 
                                     $evenPageHeading, $documentTitle)"/>
    <xsl:variable name="hce" 
      select="u:expandHeaderOrFooter($header-center,
                                     $pageNumber, true(), $pageCount, 
                                     $evenPageHeading, $documentTitle)"/>
    <xsl:variable name="hre" 
      select="u:expandHeaderOrFooter($header-right,
                                     $pageNumber, true(), $pageCount, 
                                     $evenPageHeading, $documentTitle)"/>

    <xsl:if test="exists($hle) or exists($hce) or exists($hre)">
      <fo:static-content flow-name="even-page-header">
        <xsl:call-template name="tabularHeader">
          <xsl:with-param name="left" select="$hle"/>
          <xsl:with-param name="center" select="$hce"/>
          <xsl:with-param name="right" select="$hre"/>
          <xsl:with-param name="isFooter" select="false()"/>
        </xsl:call-template>
      </fo:static-content>
    </xsl:if>

    <xsl:variable name="fl1" 
      select="u:expandHeaderOrFooter($footer-left,
                                     $pageNumber, false(), $pageCount, 
                                     $firstPageHeading, $documentTitle)"/>
    <xsl:variable name="fc1" 
      select="u:expandHeaderOrFooter($footer-center,
                                     $pageNumber, false(), $pageCount, 
                                     $firstPageHeading, $documentTitle)"/>
    <xsl:variable name="fr1" 
      select="u:expandHeaderOrFooter($footer-right,
                                     $pageNumber, false(), $pageCount, 
                                     $firstPageHeading, $documentTitle)"/>

    <xsl:if test="exists($fl1) or exists($fc1) or exists($fr1)">
      <fo:static-content flow-name="first-page-footer">
        <xsl:call-template name="tabularHeader">
          <xsl:with-param name="left" select="$fl1"/>
          <xsl:with-param name="center" select="$fc1"/>
          <xsl:with-param name="right" select="$fr1"/>
          <xsl:with-param name="isFooter" select="true()"/>
        </xsl:call-template>
      </fo:static-content>
    </xsl:if>

    <xsl:variable name="flo" 
      select="u:expandHeaderOrFooter($footer-left,
                                     $pageNumber, false(), $pageCount, 
                                     $oddPageHeading, $documentTitle)"/>
    <xsl:variable name="fco" 
      select="u:expandHeaderOrFooter($footer-center,
                                     $pageNumber, false(), $pageCount, 
                                     $oddPageHeading, $documentTitle)"/>
    <xsl:variable name="fro" 
      select="u:expandHeaderOrFooter($footer-right,
                                     $pageNumber, false(), $pageCount, 
                                     $oddPageHeading, $documentTitle)"/>

    <xsl:if test="exists($flo) or exists($fco) or exists($fro)">
      <fo:static-content flow-name="odd-page-footer">
        <xsl:call-template name="tabularHeader">
          <xsl:with-param name="left" select="$flo"/>
          <xsl:with-param name="center" select="$fco"/>
          <xsl:with-param name="right" select="$fro"/>
          <xsl:with-param name="isFooter" select="true()"/>
        </xsl:call-template>
      </fo:static-content>
    </xsl:if>

    <xsl:variable name="fle" 
      select="u:expandHeaderOrFooter($footer-left, 
                                     $pageNumber, true(), $pageCount, 
                                     $evenPageHeading, $documentTitle)"/>
    <xsl:variable name="fce" 
      select="u:expandHeaderOrFooter($footer-center, 
                                     $pageNumber, true(), $pageCount, 
                                     $evenPageHeading, $documentTitle)"/>
    <xsl:variable name="fre" 
      select="u:expandHeaderOrFooter($footer-right, 
                                     $pageNumber, true(), $pageCount, 
                                     $evenPageHeading, $documentTitle)"/>

    <xsl:if test="exists($fle) or exists($fce) or exists($fre)">
      <fo:static-content flow-name="even-page-footer">
        <xsl:call-template name="tabularHeader">
          <xsl:with-param name="left" select="$fle"/>
          <xsl:with-param name="center" select="$fce"/>
          <xsl:with-param name="right" select="$fre"/>
          <xsl:with-param name="isFooter" select="true()"/>
        </xsl:call-template>
      </fo:static-content>
    </xsl:if>

  </xsl:template>

  <xsl:function name="u:expandHeaderOrFooter" as="item()*">
    <xsl:param name="spec" as="xs:string"/>
    <xsl:param name="pageNumber" as="item()*"/>
    <xsl:param name="isEven" as="xs:boolean"/>
    <xsl:param name="pageCount" as="item()*"/>
    <xsl:param name="heading" as="item()*"/>
    <xsl:param name="documentTitle" as="item()*"/>
    
    <xsl:variable name="oddPageNumber"
                  select="if ($two-sided eq 'yes' and $isEven)
                          then ()
                          else $pageNumber"/>

    <xsl:variable name="evenPageNumber"
                  select="if ($two-sided eq 'yes' and $isEven)
                          then $pageNumber
                          else ()"/>

    <xsl:variable name="lineBreak">
      <fo:block/>
    </xsl:variable>

    <!-- The Private Use Area of the Unicode Basic Multilingual Plane is 
         E000 to F8FF. 
         We'll use:
         E000 = separator 
         E001 = page-number
         E002 = odd-page-number
         E003 = even-page-number
         E004 = page-count
         E005 = current-heading
         E006 = document-title
         E007 = break
         E008 = image()
         E009 = url() -->

    <xsl:variable name="spec1" 
      select="if (contains($spec, '{{page-number}}'))
              then replace($spec, '\{\{page-number\}\}', 
                           '&#xE000;&#xE001;&#xE000;')
              else $spec"/>
    <xsl:variable name="spec2" 
      select="if (contains($spec1, '{{odd-page-number}}'))
              then replace($spec1, '\{\{odd-page-number\}\}', 
                           '&#xE000;&#xE002;&#xE000;')
              else $spec1"/>
    <xsl:variable name="spec3" 
      select="if (contains($spec2, '{{even-page-number}}'))
              then replace($spec2, '\{\{even-page-number\}\}', 
                           '&#xE000;&#xE003;&#xE000;')
              else $spec2"/>
    <xsl:variable name="spec4" 
      select="if (contains($spec3, '{{page-count}}'))
              then replace($spec3, '\{\{page-count\}\}', 
                           '&#xE000;&#xE004;&#xE000;')
              else $spec3"/>
    <xsl:variable name="spec5" 
      select="if (contains($spec4, '{{current-heading}}'))
              then replace($spec4, '\{\{current-heading\}\}', 
                           '&#xE000;&#xE005;&#xE000;')
              else $spec4"/>
    <xsl:variable name="spec6" 
      select="if (contains($spec5, '{{document-title}}'))
              then replace($spec5, '\{\{document-title\}\}', 
                           '&#xE000;&#xE006;&#xE000;')
              else $spec5"/>
    <xsl:variable name="spec7" 
      select="if (contains($spec6, '{{break}}'))
              then replace($spec6, '\{\{break\}\}', '&#xE000;&#xE007;&#xE000;')
              else $spec6"/>
    <xsl:variable name="spec8" 
      select="if (contains($spec7, '{{image(' ))
              then replace($spec7, '\{\{image\(([^)]+)\)\}\}', 
                           '&#xE000;&#xE008;$1&#xE000;')
              else $spec7"/>
    <xsl:variable name="spec9" 
      select="if (contains($spec8, '{{url(' ))
              then replace($spec8, '\{\{url\(([^)]+)\)\}\}', 
                           '&#xE000;&#xE009;$1&#xE000;')
              else $spec8"/>

    <xsl:sequence 
      select="for $t in tokenize($spec9, '&#xE000;')
              return
                if ($t eq '&#xE001;') then $pageNumber
                else 
                  if ($t eq '&#xE002;') then $oddPageNumber
                  else 
                    if ($t eq '&#xE003;') then $evenPageNumber
                    else 
                      if ($t eq '&#xE004;') then $pageCount
                      else 
                        if ($t eq '&#xE005;') then $heading
                        else 
                          if ($t eq '&#xE006;') then $documentTitle
                          else 
                            if ($t eq '&#xE007;') then $lineBreak
                            else 
                              if (starts-with($t, '&#xE008;')) 
                              then u:toExternalGraphic(substring($t, 2))
                              else
                                if (starts-with($t, '&#xE009;')) 
                                then u:toExternalLink(substring($t, 2))
                                else 
                                  if ($t ne '') then $t else ()"/>
  </xsl:function>

  <xsl:function name="u:toExternalGraphic" as="element()">
    <xsl:param name="uri" as="xs:string"/>

    <fo:external-graphic src="{concat('url(', u:toAbsoluteURI($uri), ')')}"/>
  </xsl:function>

  <xsl:attribute-set name="link-in-header-or-footer"
                     use-attribute-sets="link-style">
  </xsl:attribute-set>

  <xsl:function name="u:toExternalLink" as="element()">
    <xsl:param name="spec" as="xs:string"/>

    <xsl:variable name="spec2" select="normalize-space($spec)"/>
    <xsl:variable name="spec3" 
                  select="if (contains($spec2, ' ')) 
                          then $spec2
                          else concat($spec2, ' ')"/>
    <xsl:variable name="uri" 
                  select="substring-before($spec3, ' ')"/>
    <xsl:variable name="text" 
                  select="substring-after($spec3, ' ')"/>

    <fo:basic-link external-destination="{concat('url(', $uri, ')')}"
                   xsl:use-attribute-sets="link-in-header-or-footer">
      <xsl:value-of select="if ($text eq '') then $uri else $text"/>
    </fo:basic-link>
  </xsl:function>

  <!-- tabularHeader ===================================================== -->

  <xsl:attribute-set name="page-header-or-footer">
    <xsl:attribute name="hyphenate">false</xsl:attribute>
    <xsl:attribute name="font-family">sans-serif</xsl:attribute>
    <xsl:attribute name="font-size">0.67em</xsl:attribute>
    <xsl:attribute name="color">#404040</xsl:attribute>
    <xsl:attribute name="border-color">#404040</xsl:attribute>
    <xsl:attribute name="border-width">0.25pt</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="page-header" 
                     use-attribute-sets="page-header-or-footer"/>

  <xsl:attribute-set name="page-footer" 
                     use-attribute-sets="page-header-or-footer"/>

  <xsl:attribute-set name="page-header-or-footer-left">
    <xsl:attribute name="text-align">left</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="page-header-or-footer-center">
    <xsl:attribute name="text-align">center</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="page-header-or-footer-right">
    <xsl:attribute name="text-align">right</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="page-header-left" 
                     use-attribute-sets="page-header-or-footer-left"/>
  <xsl:attribute-set name="page-header-center" 
                     use-attribute-sets="page-header-or-footer-center"/>
  <xsl:attribute-set name="page-header-right" 
                     use-attribute-sets="page-header-or-footer-right"/>

  <xsl:attribute-set name="page-footer-left" 
                     use-attribute-sets="page-header-or-footer-left"/>
  <xsl:attribute-set name="page-footer-center" 
                     use-attribute-sets="page-header-or-footer-center"/>
  <xsl:attribute-set name="page-footer-right" 
                     use-attribute-sets="page-header-or-footer-right"/>

  <xsl:template name="tabularHeader">
    <xsl:param name="left" select="()"/>
    <xsl:param name="center" select="()"/>
    <xsl:param name="right" select="()"/>
    <xsl:param name="isFooter" select="false()"/>

    <xsl:choose>
      <xsl:when test="$isFooter">
        <fo:block xsl:use-attribute-sets="page-footer">
          <xsl:if test="$footer-separator eq 'yes'">
            <xsl:attribute name="border-top-style">solid</xsl:attribute>
            <xsl:attribute name="padding-top">0.125em</xsl:attribute>
          </xsl:if>

          <fo:table table-layout="fixed" width="100%">
            <fo:table-column column-number="1" 
                             column-width="{concat('proportional-column-width(',
                                                   $footer-left-width, 
                                                   ')')}"/>
            <fo:table-column column-number="2" 
                             column-width="{concat('proportional-column-width(',
                                                   $footer-center-width, 
                                                   ')')}"/>
            <fo:table-column column-number="3" 
                             column-width="{concat('proportional-column-width(',
                                                   $footer-right-width, 
                                                   ')')}"/>
            <fo:table-body>
              <fo:table-row>
                <fo:table-cell start-indent="0" display-align="before"
                               xsl:use-attribute-sets="page-footer-left">
                  <fo:block>
                    <xsl:choose>
                      <xsl:when test="exists($left)">
                        <xsl:copy-of select="$left"/>
                      </xsl:when>
                      <xsl:otherwise>&#xA0;</xsl:otherwise>
                    </xsl:choose>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell start-indent="0" display-align="before"
                               xsl:use-attribute-sets="page-footer-center">
                  <fo:block>
                    <xsl:choose>
                      <xsl:when test="exists($center)">
                        <xsl:copy-of select="$center"/>
                      </xsl:when>
                      <xsl:otherwise>&#xA0;</xsl:otherwise>
                    </xsl:choose>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell start-indent="0" display-align="before"
                               xsl:use-attribute-sets="page-footer-right">
                  <fo:block>
                    <xsl:choose>
                      <xsl:when test="exists($right)">
                        <xsl:copy-of select="$right"/>
                      </xsl:when>
                      <xsl:otherwise>&#xA0;</xsl:otherwise>
                    </xsl:choose>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>
            </fo:table-body>
          </fo:table>
        </fo:block>
      </xsl:when>

      <xsl:otherwise>
        <fo:block xsl:use-attribute-sets="page-header">
          <xsl:if test="$header-separator eq 'yes'">
            <xsl:attribute name="padding-bottom">0.125em</xsl:attribute>
            <xsl:attribute name="border-bottom-style">solid</xsl:attribute>
          </xsl:if>

          <fo:table table-layout="fixed" width="100%">
            <fo:table-column column-number="1" 
                             column-width="{concat('proportional-column-width(',
                                                   $header-left-width, 
                                                   ')')}"/>
            <fo:table-column column-number="2" 
                             column-width="{concat('proportional-column-width(',
                                                   $header-center-width, 
                                                   ')')}"/>
            <fo:table-column column-number="3" 
                             column-width="{concat('proportional-column-width(',
                                                   $header-right-width, 
                                                   ')')}"/>
            <fo:table-body>
              <fo:table-row>
                <fo:table-cell start-indent="0" display-align="after"
                               xsl:use-attribute-sets="page-header-left">
                  <fo:block>
                    <xsl:choose>
                      <xsl:when test="exists($left)">
                        <xsl:copy-of select="$left"/>
                      </xsl:when>
                      <xsl:otherwise>&#xA0;</xsl:otherwise>
                    </xsl:choose>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell start-indent="0" display-align="after"
                               xsl:use-attribute-sets="page-header-center">
                  <fo:block>
                    <xsl:choose>
                      <xsl:when test="exists($center)">
                        <xsl:copy-of select="$center"/>
                      </xsl:when>
                      <xsl:otherwise>&#xA0;</xsl:otherwise>
                    </xsl:choose>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell start-indent="0" display-align="after"
                               xsl:use-attribute-sets="page-header-right">
                  <fo:block>
                    <xsl:choose>
                      <xsl:when test="exists($right)">
                        <xsl:copy-of select="$right"/>
                      </xsl:when>
                      <xsl:otherwise>&#xA0;</xsl:otherwise>
                    </xsl:choose>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>
            </fo:table-body>
          </fo:table>
        </fo:block>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
