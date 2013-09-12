<?xml version="1.0" encoding="UTF-8"?>

<!-- =====================================================================
     XSLT 2 stylesheets for XHTML 1.0, 1.1, 5.0.
     Templates for the table element and its descendant elements.

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
                exclude-result-prefixes="xs html u"
                version="2.0">

  <!-- Tabular data ====================================================== -->

  <!-- table ========== -->

  <xsl:attribute-set name="table-with-caption">
  </xsl:attribute-set>

  <xsl:attribute-set name="table-border-style">
    <xsl:attribute name="border-width">0.5pt</xsl:attribute>
    <xsl:attribute name="border-color">gray</xsl:attribute>
    <xsl:attribute name="border-top-style">none</xsl:attribute>
    <xsl:attribute name="border-right-style">none</xsl:attribute>
    <xsl:attribute name="border-bottom-style">none</xsl:attribute>
    <xsl:attribute name="border-left-style">none</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="table" use-attribute-sets="table-border-style">
    <!-- Inherited default value recommended by the HTML standard. -->
    <xsl:attribute name="display-align">center</xsl:attribute>
    <xsl:attribute name="border-collapse">separate</xsl:attribute>
    <xsl:attribute
      name="border-separation.inline-progression-direction">1pt</xsl:attribute>
    <xsl:attribute
      name="border-separation.block-progression-direction">1pt</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:table">
    <xsl:choose>
      <xsl:when test="exists(./html:caption)">
        <xsl:variable name="captionSide"
                      select="u:captionSide(./html:caption)"/>

        <xsl:choose>
          <xsl:when test="$foProcessor eq 'FOP'">
            <!-- FOP does not support fo:table-and-caption -->
            <fo:block xsl:use-attribute-sets="table-with-caption">
              <xsl:call-template name="cssStyles">
                <xsl:with-param name="select" select="'margin-'"/>
              </xsl:call-template>

              <xsl:if test="$captionSide ne 'bottom'">
                <xsl:apply-templates select="html:caption"/>
              </xsl:if>

              <fo:table xsl:use-attribute-sets="table">
                <xsl:call-template name="tableContents"/>
              </fo:table>

              <xsl:if test="$captionSide eq 'bottom'">
                <xsl:apply-templates select="html:caption"/>
              </xsl:if>
            </fo:block>
          </xsl:when>

          <xsl:otherwise>
            <fo:table-and-caption xsl:use-attribute-sets="table-with-caption">
              <xsl:call-template name="cssStyles">
                <xsl:with-param name="select" select="'margin-'"/>
              </xsl:call-template>

              <xsl:if test="$captionSide eq 'bottom'">
                <xsl:attribute name="caption-side" select="'after'" />
              </xsl:if>
              <!-- Otherwise default is 'before'. -->

              <xsl:apply-templates select="html:caption"/>

              <fo:table xsl:use-attribute-sets="table">
                <xsl:call-template name="tableContents"/>
              </fo:table>
            </fo:table-and-caption>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>

      <xsl:otherwise>
        <fo:table xsl:use-attribute-sets="table">
          <xsl:call-template name="tableContents"/>
        </fo:table>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="tableContents">
      <!-- FOP does not support table-layout=auto and needs a width. -->
    <xsl:if test="$foProcessor eq 'FOP'">
      <xsl:attribute name="width">100%</xsl:attribute>
    </xsl:if>

    <!--LEGACY-->
    <xsl:if test="exists(@width)">
      <xsl:attribute name="width" select="u:asLengthOrPercent(@width)"/>
    </xsl:if>

    <!--LEGACY-->
    <xsl:if test="number(@cellspacing) ge 0">
      <xsl:attribute name="border-collapse">separate</xsl:attribute>
      <xsl:attribute name="border-separation.inline-progression-direction" 
                     select="u:asLength(@cellspacing)"/>
      <xsl:attribute name="border-separation.block-progression-direction" 
                     select="u:asLength(@cellspacing)"/>
    </xsl:if>

    <!--LEGACY-->
    <xsl:variable name="rules">
      <xsl:call-template name="tableRules"/>
    </xsl:variable>
    <xsl:if test="$rules eq 'groups' or
                  $rules eq 'rows' or
                  $rules eq 'cols' or
                  $rules eq 'all'">
      <xsl:attribute name="border-collapse">collapse</xsl:attribute>
      <xsl:attribute
       name="border-separation.inline-progression-direction">0pt</xsl:attribute>
      <xsl:attribute
       name="border-separation.block-progression-direction">0pt</xsl:attribute>
    </xsl:if>

    <!--LEGACY-->
    <xsl:if test="number(@border) gt 1">
      <xsl:attribute name="border-width" select="u:asLength(@border)"/>
    </xsl:if>
    <!-- For @border=1, use the width specified in the attribute-set. -->

    <!--LEGACY-->
    <xsl:variable name="frame" as="xs:string">
      <xsl:choose>
        <xsl:when test="exists(@border)">
          <xsl:choose>
            <xsl:when test="@border eq '0'">void</xsl:when>
            <xsl:otherwise>border</xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:choose>
            <xsl:when test="@frame eq 'void' or
                            @frame eq 'above' or
                            @frame eq 'below' or
                            @frame eq 'hsides' or
                            @frame eq 'lhs' or
                            @frame eq 'rhs' or
                            @frame eq 'vsides' or
                            @frame eq 'box' or
                            @frame eq 'border'">
              <xsl:value-of select="@frame"/>
            </xsl:when>
            <xsl:otherwise>void</xsl:otherwise>
          </xsl:choose>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$frame eq 'void'">
        <xsl:attribute name="border-top-style">none</xsl:attribute>
        <xsl:attribute name="border-right-style">none</xsl:attribute>
        <xsl:attribute name="border-bottom-style">none</xsl:attribute>
        <xsl:attribute name="border-left-style">none</xsl:attribute>
      </xsl:when>
      <xsl:when test="$frame eq 'above'">
        <xsl:attribute name="border-top-style">solid</xsl:attribute>
        <xsl:attribute name="border-right-style">none</xsl:attribute>
        <xsl:attribute name="border-bottom-style">none</xsl:attribute>
        <xsl:attribute name="border-left-style">none</xsl:attribute>
      </xsl:when>
      <xsl:when test="$frame eq 'below'">
        <xsl:attribute name="border-top-style">none</xsl:attribute>
        <xsl:attribute name="border-right-style">none</xsl:attribute>
        <xsl:attribute name="border-bottom-style">solid</xsl:attribute>
        <xsl:attribute name="border-left-style">none</xsl:attribute>
      </xsl:when>
      <xsl:when test="$frame eq 'hsides'">
        <xsl:attribute name="border-top-style">solid</xsl:attribute>
        <xsl:attribute name="border-right-style">none</xsl:attribute>
        <xsl:attribute name="border-bottom-style">solid</xsl:attribute>
        <xsl:attribute name="border-left-style">none</xsl:attribute>
      </xsl:when>
      <xsl:when test="$frame eq 'lhs'">
        <xsl:attribute name="border-top-style">none</xsl:attribute>
        <xsl:attribute name="border-right-style">none</xsl:attribute>
        <xsl:attribute name="border-bottom-style">none</xsl:attribute>
        <xsl:attribute name="border-left-style">solid</xsl:attribute>
      </xsl:when>
      <xsl:when test="$frame eq 'rhs'">
        <xsl:attribute name="border-top-style">none</xsl:attribute>
        <xsl:attribute name="border-right-style">solid</xsl:attribute>
        <xsl:attribute name="border-bottom-style">none</xsl:attribute>
        <xsl:attribute name="border-left-style">none</xsl:attribute>
      </xsl:when>
      <xsl:when test="$frame eq 'vsides'">
        <xsl:attribute name="border-top-style">none</xsl:attribute>
        <xsl:attribute name="border-right-style">solid</xsl:attribute>
        <xsl:attribute name="border-bottom-style">none</xsl:attribute>
        <xsl:attribute name="border-left-style">solid</xsl:attribute>
      </xsl:when>
      <xsl:otherwise>
        <!-- box or border -->
        <xsl:attribute name="border-top-style">solid</xsl:attribute>
        <xsl:attribute name="border-right-style">solid</xsl:attribute>
        <xsl:attribute name="border-bottom-style">solid</xsl:attribute>
        <xsl:attribute name="border-left-style">solid</xsl:attribute>
      </xsl:otherwise>
    </xsl:choose>

    <!--LEGACY-->
    <xsl:call-template name="bgcolorAttribute"/>

    <xsl:if test="empty(./html:caption)">
      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'margin-'"/>
      </xsl:call-template>
    </xsl:if>
    <xsl:call-template name="cssStyles">
      <xsl:with-param name="select" select="'table'"/>
    </xsl:call-template>

    <xsl:call-template name="commonAttributes"/>

    <xsl:if test="$foProcessor eq 'FOP' or exists(html:colgroup|html:col)">
      <!-- FOP does not support table-layout=auto -->
      <xsl:attribute name="table-layout">fixed</xsl:attribute>

      <xsl:variable name="columns" as="element()*">
        <xsl:apply-templates select="html:colgroup|html:col"/>
      </xsl:variable>
      <xsl:variable name="columnCount" select="count($columns)" 
                    as="xs:integer"/>

      <xsl:variable name="actualColumnCount" as="xs:integer">
        <xsl:call-template name="countColumns"/>
      </xsl:variable>

      <xsl:choose>
        <xsl:when test="$columnCount gt $actualColumnCount">
          <xsl:sequence 
            select="subsequence($columns, 1, $actualColumnCount)"/>
        </xsl:when>
        <xsl:when test="$columnCount lt $actualColumnCount">
          <xsl:variable name="extraColumns">
            <xsl:for-each select="($columnCount + 1) to $actualColumnCount">
              <fo:table-column xsl:use-attribute-sets="col" 
                               column-width="proportional-column-width(1)"/>
            </xsl:for-each>
          </xsl:variable>
          <xsl:sequence select="($columns, $extraColumns)"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:sequence select="$columns"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>

    <xsl:apply-templates select="html:thead"/>
    <xsl:apply-templates select="html:tfoot"/>

    <xsl:choose>
      <xsl:when test="exists(html:tbody)">
        <xsl:apply-templates select="html:tbody"/>
      </xsl:when>
      <xsl:otherwise>
        <fo:table-body xsl:use-attribute-sets="tbody">
          <xsl:apply-templates select="html:tr"/>
        </fo:table-body>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--LEGACY-->
  <xsl:template name="tableRules" as="xs:string">
    <xsl:for-each select="ancestor-or-self::html:table[1]">
      <xsl:variable name="rules">
        <xsl:choose>
          <xsl:when test="@rules eq 'none' or
                          @rules eq 'groups' or
                          @rules eq 'rows' or
                          @rules eq 'cols' or
                          @rules eq 'all'">
            <xsl:value-of select="@rules"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:choose>
        <xsl:when test="exists(@border)">
          <xsl:choose>
            <xsl:when test="@border eq '0'">
              <xsl:value-of select="if ($rules ne '') then $rules else 'none'"/>
            </xsl:when>
            <xsl:when test="number(@border) gt 0">
              <xsl:value-of select="if ($rules ne '') then $rules else 'all'"/>
            </xsl:when>
            <!-- Example: @border="border". -->
            <xsl:otherwise>all</xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="if ($rules ne '') then $rules else 'none'"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="countColumns" as="xs:integer">
    <xsl:variable name="spans" as="item()*">
      <xsl:for-each select="(.//html:tr)[1]">
        <xsl:for-each select="./html:td|./html:th">
          <xsl:choose>
            <xsl:when test="number(@colspan) gt 0">
              <xsl:sequence select="xs:integer(@colspan)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:sequence select="1"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:for-each>
      </xsl:for-each>
    </xsl:variable>

    <xsl:sequence select="xs:integer(sum($spans))"/>
  </xsl:template>

  <!-- caption ========== -->

  <xsl:attribute-set name="table-caption">
    <xsl:attribute name="text-align">center</xsl:attribute>
    <xsl:attribute name="hyphenate">false</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:caption">
    <xsl:choose>
      <xsl:when test="$foProcessor eq 'FOP'">
        <xsl:variable name="captionSide" select="u:captionSide(.)"/>

        <!-- FOP does not support fo:table-and-caption -->
        <fo:block xsl:use-attribute-sets="table-caption">
          <xsl:choose>
            <xsl:when test="$captionSide eq 'bottom'">
              <xsl:attribute
                name="keep-with-previous.within-column">always</xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute
                name="keep-with-next.within-column">always</xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>

          <!-- Block, not table-caption, styles -->
          <xsl:call-template name="cssStyles"/>

          <xsl:call-template name="commonAttributes"/>
          <xsl:apply-templates/>
        </fo:block>
      </xsl:when>

      <xsl:otherwise>
        <fo:table-caption xsl:use-attribute-sets="table-caption">
          <xsl:call-template name="cssStyles">
            <xsl:with-param name="select" select="'table-caption'"/>
          </xsl:call-template>

          <xsl:call-template name="commonAttributes"/>
          <fo:block>
            <xsl:apply-templates/>
          </fo:block>
        </fo:table-caption>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- colgroup ========== -->

  <xsl:template match="html:colgroup">
    <xsl:variable name="isFirstGroup" 
                  select="empty(preceding-sibling::html:colgroup)"/>
    <xsl:variable name="isLastGroup" 
                  select="empty(following-sibling::html:colgroup)"/>

    <xsl:choose>
      <xsl:when test="exists(./html:col)">
        <xsl:variable name="colCount" select="count(./html:col)"/>

        <xsl:for-each select="./html:col">
          <xsl:variable name="colPos" select="position()"/>

          <fo:table-column xsl:use-attribute-sets="col">
            <xsl:call-template name="tableColumnContents">
              <xsl:with-param name="drawGroupLeftEdge" 
                select="($colPos eq 1) and not($isFirstGroup)"/>
              <xsl:with-param name="drawGroupRightEdge" 
                select="($colPos eq $colCount) and not($isLastGroup)"/>
            </xsl:call-template>
          </fo:table-column>
        </xsl:for-each>
      </xsl:when>

      <xsl:otherwise>
        <xsl:variable name="span" as="xs:integer">
          <xsl:call-template name="columnSpan"/>
        </xsl:variable>

        <xsl:variable name="thisColgroup" select="."/>

        <xsl:for-each select="1 to $span">
          <xsl:variable name="pos" select="position()"/>

          <xsl:for-each select="$thisColgroup">
            <fo:table-column xsl:use-attribute-sets="col">
              <xsl:call-template name="tableColumnContents">
                <xsl:with-param name="drawGroupLeftEdge" 
                  select="($pos eq 1) and not($isFirstGroup)"/>
                <xsl:with-param name="drawGroupRightEdge" 
                  select="($pos eq $span) and not($isLastGroup)"/>
              </xsl:call-template>
            </fo:table-column>
          </xsl:for-each>
        </xsl:for-each>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="columnSpan" as="xs:integer">
    <xsl:choose>
      <xsl:when test="number(@span) gt 0">
        <xsl:sequence select="xs:integer(@span)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:sequence select="1"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- col ========== -->

  <xsl:attribute-set name="col" use-attribute-sets="table-border-style">
    <!-- Default value recommended by the HTML standard. -->
    <xsl:attribute name="display-align">center</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:col">
    <xsl:variable name="span" as="xs:integer">
      <xsl:call-template name="columnSpan"/>
    </xsl:variable>

    <xsl:variable name="thisCol" select="."/>

    <xsl:for-each select="1 to $span">
      <xsl:for-each select="$thisCol">
        <fo:table-column xsl:use-attribute-sets="col">
          <xsl:call-template name="tableColumnContents"/>
        </fo:table-column>
      </xsl:for-each>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="tableColumnContents">
    <xsl:param name="drawGroupLeftEdge" select="false()" as="xs:boolean"/>
    <xsl:param name="drawGroupRightEdge" select="false()" as="xs:boolean"/>

    <xsl:if test="$foProcessor eq 'FOP' or $foProcessor eq 'XFC'">
      <!-- 
           * FOP reports a warning is the width is absent and it automatically 
             sets it to proportional-column-width(1).
           * XFC needs all table-column widths to be specified, otherwise
             it fallbacks to table-layout=auto.
      -->
      <xsl:attribute
        name="column-width">proportional-column-width(1)</xsl:attribute>
    </xsl:if>

    <xsl:for-each select="parent::html:colgroup">
      <!--LEGACY-->
      <xsl:call-template name="colWidthAttribute" />
      <xsl:call-template name="cellAlignAttribute" />
      <xsl:call-template name="cellValignAttribute" />
    </xsl:for-each>

    <!--LEGACY-->
    <xsl:call-template name="colWidthAttribute" />
    <xsl:call-template name="cellAlignAttribute" />
    <xsl:call-template name="cellValignAttribute" />

    <!--LEGACY-->
    <xsl:if test="$drawGroupLeftEdge or $drawGroupRightEdge">
      <xsl:variable name="rules">
        <xsl:call-template name="tableRules"/>
      </xsl:variable>
      <xsl:if test="$rules eq 'groups'">
        <xsl:if test="$drawGroupLeftEdge">
          <xsl:attribute name="border-left-style">solid</xsl:attribute>
        </xsl:if>
        <xsl:if test="$drawGroupRightEdge">
          <xsl:attribute name="border-right-style">solid</xsl:attribute>
        </xsl:if>
      </xsl:if>
      <!-- rules=all handled by th/td -->
    </xsl:if>

    <xsl:call-template name="cssStyles">
      <xsl:with-param name="select" select="'table-column'"/>
    </xsl:call-template>

    <!-- A fo:table-column cannot have id, xml:lang, etc, attributes. -->
    <!-- A fo:table-column is empty. -->
  </xsl:template>

  <!--LEGACY-->
  <xsl:template name="colWidthAttribute">
    <xsl:variable name="cssWidth" select="u:cssStyle(., 'width')"/>
    <xsl:variable name="width"
                  select="if ($cssWidth ne '' and 
                              $cssWidth ne 'auto' and 
                              $cssWidth ne 'inherit')
                          then $cssWidth
                          else 
                              if (exists(@width) and @width ne '0*')
                              then string(@width)
                              else ''"/>

    <xsl:if test="$width ne ''">
      <xsl:attribute name="column-width">
        <xsl:choose>
          <xsl:when test="ends-with($width, '*')">
            <xsl:text>proportional-column-width(</xsl:text>
            <xsl:value-of select="substring-before($width, '*')"/>
            <xsl:text>)</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="u:asLengthOrPercent($width)"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
    </xsl:if>
  </xsl:template>

  <!--LEGACY-->
  <xsl:template name="cellAlignAttribute">
    <xsl:if test="exists(@align)">
      <xsl:attribute name="text-align">
        <xsl:choose>
          <xsl:when test="@align eq 'char'">
            <xsl:choose>
              <xsl:when test="exists(@align/../@char)">
                <xsl:value-of select="@align/../@char"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:variable name="lang"
                  select="lower-case(
                            if (exists(ancestor-or-self::*/@*:lang))
                            then string((ancestor-or-self::*/@*:lang)[last()])
                            else 'en')"/>
    
                <xsl:value-of select="u:decimalSeparator($lang)"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="@align eq 'center' or
                          @align eq 'right' or
                          @align eq 'justify'">
            <xsl:value-of select="@align"/>
          </xsl:when>
          <xsl:otherwise>left</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
    </xsl:if>
  </xsl:template>

  <xsl:function name="u:decimalSeparator" as="xs:string">
    <xsl:param name="lang" as="xs:string"/>

    <xsl:variable name="lang2" select="substring($lang, 1, 2)"/>

    <xsl:choose>
      <xsl:when test="$lang2 eq 'fr' or
                      $lang2 eq 'de' or
                      $lang2 eq 'es' or
                      $lang2 eq 'nl' or
                      $lang2 eq 'sv' or
                      $lang2 eq 'no' or
                      $lang2 eq 'pl' or
                      $lang2 eq 'ru' or
                      $lang2 eq 'it' or
                      $lang2 eq 'hu' or
                      $lang2 eq 'el' or
                      $lang2 eq 'fi' or
                      $lang2 eq 'sk' or
                      $lang2 eq 'sl' or
                      $lang2 eq 'tr' or
                      $lang2 eq 'cs'">
        <xsl:sequence select="','"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:sequence select="'.'"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>

  <!--LEGACY-->
  <xsl:template name="cellValignAttribute">
    <xsl:if test="exists(@valign)">
      <xsl:attribute name="display-align">
        <xsl:choose>
          <xsl:when test="@valign eq 'middle'">center</xsl:when>
          <xsl:when test="@valign eq 'bottom'">after</xsl:when>
          <xsl:when test="@valign eq 'baseline'">auto</xsl:when>
          <xsl:otherwise>before</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>

      <xsl:if test="@valign eq 'baseline'">
        <xsl:attribute name="relative-align">baseline</xsl:attribute>
      </xsl:if>
    </xsl:if>
  </xsl:template>

  <!-- thead ========== -->

  <xsl:attribute-set name="thead" use-attribute-sets="table-border-style">
  </xsl:attribute-set>

  <xsl:template match="html:thead">
    <fo:table-header xsl:use-attribute-sets="thead">
      <xsl:call-template name="tableBodyContents"/>
    </fo:table-header>
  </xsl:template>

  <xsl:template name="tableBodyContents">
    <!--LEGACY-->
    <xsl:variable name="rules">
      <xsl:call-template name="tableRules"/>
    </xsl:variable>
    <xsl:if test="$rules eq 'groups' or $rules eq 'all'">
      <xsl:if test="exists(self::html:tfoot) or
                    (exists(self::html:tbody) and
                     exists(preceding-sibling::html:thead|
                            preceding-sibling::html:tbody))">
        <xsl:attribute name="border-top-style">solid</xsl:attribute>
      </xsl:if>
      <xsl:if test="exists(self::html:thead) or
                    (exists(self::html:tbody) and
                     exists(preceding-sibling::html:tfoot|
                            following-sibling::html:tfoot|
                            following-sibling::html:tbody))">
        <xsl:attribute name="border-bottom-style">solid</xsl:attribute>
      </xsl:if>
    </xsl:if>

    <!--LEGACY-->
    <xsl:call-template name="bgcolorAttribute"/>

    <!-- @align, @valign, text-align, vertical-align handled by the cell. -->

    <xsl:call-template name="cssStyles">
      <xsl:with-param name="select" select="'table-row-group'"/>
    </xsl:call-template>

    <xsl:call-template name="commonAttributes"/>
    <xsl:apply-templates/>
  </xsl:template>

  <!-- tfoot ========== -->

  <xsl:attribute-set name="tfoot" use-attribute-sets="table-border-style">
  </xsl:attribute-set>

  <xsl:template match="html:tfoot">
    <fo:table-footer xsl:use-attribute-sets="tfoot">
      <xsl:call-template name="tableBodyContents"/>
    </fo:table-footer>
  </xsl:template>

  <!-- tbody ========== -->

  <xsl:attribute-set name="tbody" use-attribute-sets="table-border-style">
  </xsl:attribute-set>

  <xsl:template match="html:tbody">
    <fo:table-body xsl:use-attribute-sets="tbody">
      <xsl:call-template name="tableBodyContents"/>
    </fo:table-body>
  </xsl:template>

  <!-- tr ========== -->

  <xsl:attribute-set name="tr" use-attribute-sets="table-border-style">
  </xsl:attribute-set>

  <xsl:template match="html:tr">
    <fo:table-row xsl:use-attribute-sets="tr">
      <!--LEGACY-->
      <xsl:variable name="rules">
        <xsl:call-template name="tableRules"/>
      </xsl:variable>
      <xsl:if test="$rules eq 'rows' or $rules eq 'all'">
        <xsl:if test="exists(preceding-sibling::html:tr)">
          <xsl:attribute name="border-top-style">solid</xsl:attribute>
        </xsl:if>
        <xsl:if test="exists(following-sibling::html:tr)">
          <xsl:attribute name="border-bottom-style">solid</xsl:attribute>
        </xsl:if>
      </xsl:if>

      <!--LEGACY-->
      <xsl:call-template name="bgcolorAttribute"/>

      <!-- @align, @valign, text-align, vertical-align handled by the cell. -->

      <xsl:call-template name="cssStyles">
        <xsl:with-param name="select" select="'table-row'"/>
      </xsl:call-template>

      <xsl:if test="exists(parent::html:table) and 
                    exists(html:th) and 
                    empty(html:td)">
        <!-- Acts as a header. -->
        <xsl:attribute
          name="keep-with-next.within-column">always</xsl:attribute>
      </xsl:if>

      <xsl:call-template name="commonAttributes"/>
      <xsl:apply-templates/>
    </fo:table-row>
  </xsl:template>

  <!-- th ========== -->

  <xsl:attribute-set name="table-cell-style">
    <xsl:attribute name="padding">0.5pt</xsl:attribute>
  </xsl:attribute-set>

  <xsl:attribute-set name="th" 
                     use-attribute-sets="table-border-style table-cell-style">
    <xsl:attribute name="text-align">center</xsl:attribute>
    <xsl:attribute name="font-weight">bold</xsl:attribute>
  </xsl:attribute-set>

  <xsl:template match="html:th">
    <fo:table-cell xsl:use-attribute-sets="th">
      <xsl:call-template name="cellContents"/>
    </fo:table-cell>
  </xsl:template>

  <xsl:template name="cellContents">
    <!--LEGACY-->
    <xsl:variable name="rules">
      <xsl:call-template name="tableRules"/>
    </xsl:variable>
    <xsl:if test="$rules eq 'cols' or $rules eq 'all'">
      <xsl:if test="exists(preceding-sibling::html:td|
                           preceding-sibling::html:th)">
        <xsl:attribute name="border-left-style">solid</xsl:attribute>
      </xsl:if>
      <xsl:if test="exists(following-sibling::html:td|
                           following-sibling::html:th)">
        <xsl:attribute name="border-right-style">solid</xsl:attribute>
      </xsl:if>
    </xsl:if>

    <!--LEGACY-->
    <xsl:call-template name="cellPadding"/>

    <!--LEGACY-->
    <xsl:call-template name="cellSize"/>

    <!--LEGACY-->
    <xsl:call-template name="bgcolorAttribute"/>

    <!-- @align or text-align === -->

    <xsl:variable name="cellAlign" 
                  select="u:lookupCellAlign(., 'align', 'text-align')"/>
    <xsl:if test="$cellAlign ne ''">
      <xsl:attribute name="text-align">
        <xsl:choose>
          <xsl:when test="$cellAlign eq 'center' or
                          $cellAlign eq 'right' or
                          $cellAlign eq 'justify'">
            <xsl:value-of select="$cellAlign"/>
          </xsl:when>
          <xsl:otherwise>left</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
    </xsl:if>

    <xsl:if test="$foProcessor ne 'XFC'">
      <xsl:if
        test="$cellAlign eq '' and
              exists(ancestor::html:table[1]/child::*[self::html:col or
                                                      self::html:colgroup]/descendant-or-self::*/@align)">
        <!-- When a table has cols or colgroups, it is guaranteed 
             [1] to have a fixed layout [2] to have as many table-columns 
             as it has columns .-->
        <xsl:attribute name="text-align">from-table-column()</xsl:attribute>
      </xsl:if>
    </xsl:if>

    <!-- @valign or vertical-align === -->

    <xsl:variable name="cellValign"
                  select="u:lookupCellAlign(., 'valign', 'vertical-align')"/>
    <xsl:if test="$cellValign ne ''">
      <xsl:attribute name="display-align">
        <xsl:choose>
          <xsl:when test="$cellValign eq 'middle'">center</xsl:when>
          <xsl:when test="$cellValign eq 'bottom'">after</xsl:when>
          <xsl:when test="$cellValign eq 'baseline'">auto</xsl:when>
          <xsl:otherwise>before</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>

      <xsl:if test="$cellValign eq 'baseline'">
        <xsl:attribute name="relative-align">baseline</xsl:attribute>
      </xsl:if>
    </xsl:if>

    <xsl:if test="$foProcessor ne 'XFC'">
      <xsl:if
        test="$cellValign eq '' and
              exists(ancestor::html:table[1]/child::*[self::html:col or
                                                      self::html:colgroup]/descendant-or-self::*/@valign)">
        <!-- When a table has cols or colgroups, it is guaranteed 
             [1] to have a fixed layout [2] to have as many table-columns 
             as it has columns .-->
        <xsl:attribute name="display-align">from-table-column()</xsl:attribute>
        <xsl:attribute name="relative-align">from-table-column()</xsl:attribute>
      </xsl:if>
    </xsl:if>

    <xsl:call-template name="cssStyles">
      <xsl:with-param name="select" select="'table-cell'"/>
    </xsl:call-template>

    <!-- Otherwise, may be inherited. -->
    <xsl:attribute name="start-indent" select="'0pt'"/>

    <xsl:if test="number(@colspan) gt 0">
      <xsl:attribute name="number-columns-spanned" select="string(@colspan)"/>
    </xsl:if>
    <xsl:if test="number(@rowspan) gt 0">
      <xsl:attribute name="number-rows-spanned" select="string(@rowspan)"/>
    </xsl:if>

    <xsl:call-template name="commonAttributes"/>

    <fo:block>
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template name="cellPadding">
    <xsl:for-each select="ancestor::html:table[1]">
      <xsl:if test="exists(@cellpadding)">
        <xsl:attribute name="padding" 
                       select="u:asLengthOrPercent(@cellpadding)"/>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="cellSize">
    <xsl:variable name="cssWidth" select="u:cssStyle(., 'width')"/>
    <xsl:variable name="width" 
                  select="if ($cssWidth ne '') 
                          then $cssWidth 
                          else string(@width)"/>

    <xsl:if test="$width ne ''">
      <xsl:attribute name="width" select="u:asLengthOrPercent($width)"/>
    </xsl:if>

    <xsl:variable name="cssHeight" select="u:cssStyle(., 'height')"/>
    <xsl:variable name="height" 
                  select="if ($cssHeight ne '') 
                          then $cssHeight 
                          else string(@height)" />

    <xsl:if test="$height ne ''">
      <xsl:attribute name="height" select="u:asLengthOrPercent($height)"/>
    </xsl:if>
  </xsl:template>

  <xsl:function name="u:lookupCellAlign" as="xs:string">
    <xsl:param name="cell" as="element()"/>
    <xsl:param name="attrName" as="xs:string"/>
    <xsl:param name="styleName" as="xs:string"/>
    
    <xsl:variable name="cellStyle" select="u:cssStyle($cell, $styleName)"/>
    <xsl:choose>
      <xsl:when test="$cellStyle ne ''">
        <xsl:sequence select="$cellStyle"/>
      </xsl:when>
      <xsl:when test="exists($cell/attribute::*[local-name() eq $attrName])">
        <xsl:value-of select="$cell/attribute::*[local-name() eq $attrName]"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:variable name="row" select="$cell/parent::html:tr"/>

        <xsl:variable name="rowStyle" select="u:cssStyle($row, $styleName)"/>
        <xsl:choose>
          <xsl:when test="$rowStyle ne ''">
            <xsl:sequence select="$rowStyle"/>
          </xsl:when>
          <xsl:when test="exists($row/attribute::*[local-name() eq $attrName])">
            <xsl:value-of
              select="$row/attribute::*[local-name() eq $attrName]"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:variable name="tbody" 
                          select="$row/parent::html:*[self::html:thead or 
                                                      self::html:tfoot or
                                                      self::html:tbody]"/>

            <xsl:choose>
              <xsl:when test="exists($tbody)">
                <xsl:variable name="tbodyStyle" 
                              select="u:cssStyle($tbody, $styleName)"/>
                <xsl:choose>
                  <xsl:when test="$tbodyStyle ne ''">
                    <xsl:sequence select="$tbodyStyle"/>
                  </xsl:when>
                  <xsl:when
                    test="exists($tbody/attribute::*[local-name() eq $attrName])">
                    <xsl:value-of
                      select="$tbody/attribute::*[local-name() eq $attrName]"/>
                  </xsl:when>

                  <xsl:otherwise>
                    <xsl:sequence select="''"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:sequence select="''"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>

  <!-- td ========== -->

  <xsl:attribute-set name="td" 
                     use-attribute-sets="table-border-style table-cell-style">
  </xsl:attribute-set>

  <xsl:template match="html:td">
    <fo:table-cell xsl:use-attribute-sets="td">
      <xsl:call-template name="cellContents"/>
    </fo:table-cell>
  </xsl:template>

</xsl:stylesheet>
