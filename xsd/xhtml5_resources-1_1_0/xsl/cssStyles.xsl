<?xml version="1.0" encoding="UTF-8"?>

<!-- =====================================================================
     XSLT 2 stylesheets for XHTML 1.0, 1.1, 5.0.
     Convert CSS styles found in css-styles processing-instructions to
     XSL-FO presentation attributes.

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

  <!-- The following properties apply to all elements 
       and they are inherited. -->
  <xsl:param name="commonStyles" 
             select="('color', 'font-', 'line-height')"/>

  <xsl:param name="boxStyles" 
             select="('margin-', 'padding-', 'border-', 'background-')"/>

  <xsl:param name="inlineStyles" 
    select="($commonStyles, $boxStyles, 'text-decoration', 'vertical-align')"/>

  <!-- width, height processed using u:cssStyle -->
  <xsl:param name="externalGraphicStyles" 
             select="($boxStyles, 'vertical-align')"/>

  <xsl:param name="bidiOverrideStyles" select="$commonStyles"/>

  <!-- The following properties apply only to blocks, but 
       they are inherited. -->
  <xsl:param name="paragraphStyles" 
             select="('text-align', 'text-indent')"/>

  <xsl:param name="blockStyles" 
             select="($commonStyles, $boxStyles, $paragraphStyles)"/>

  <xsl:param name="inlineContainerStyles" 
             select="($commonStyles, $boxStyles)"/>

  <xsl:param name="listBlockStyles" 
             select="($commonStyles, $boxStyles)"/>

  <xsl:param name="listItemStyles" 
             select="($commonStyles, $boxStyles)"/>

  <!-- margin- properties which apply to the table-with-caption or to the
       table are handled separately. -->
  <xsl:param name="tableStyles" 
    select="($commonStyles, 'padding-', 'border-', 'background-', 
             'width', 'border-separation.')"/>

  <!-- Notice: no margin-. (Instead use padding-.) -->
  <xsl:param name="tableCaptionStyles" 
    select="($commonStyles, 'padding-', 'border-', 'background-', 
             $paragraphStyles, 'width')"/>

  <!-- width processed using u:cssStyle -->
  <xsl:param name="tableColumnStyles" 
             select="($commonStyles, 'background-')"/>

  <!-- text-align, vertical-align processed by the cell using u:cssStyle -->
  <xsl:param name="tableRowGroupStyles" 
             select="($commonStyles, 'background-')"/>

  <!-- text-align, vertical-align processed by the cell using u:cssStyle -->
  <xsl:param name="tableRowStyles" 
             select="($commonStyles, 'background-', 'height')"/>

  <!-- width, height, text-align, vertical-align processed using u:cssStyle -->
  <xsl:param name="tableCellStyles" 
    select="($commonStyles, 'padding-', 'background-', 'text-indent')"/>

  <xsl:template name="cssStyles">
    <xsl:param name="select" select="'block'"/>

    <xsl:variable name="isInline" 
                  select="$select eq 'inline' or 
                          $select eq 'external-graphic' or
                          $select eq 'bidi-override'"/>

    <xsl:if test="$apply-css-styles eq 'yes' and 
                  ./processing-instruction('css-styles')">
      <xsl:variable name="selection" 
        select="for $style in $select
                return 
                    if ($style eq 'block') then $blockStyles
                    else if ($style eq 'inline') then $inlineStyles
                    else if ($style eq 'external-graphic') then $externalGraphicStyles
                    else if ($style eq 'bidi-override') then $bidiOverrideStyles
                    else if ($style eq 'inline-container') then $inlineContainerStyles
                    else if ($style eq 'list-block') then $listBlockStyles
                    else if ($style eq 'list-item') then $listItemStyles
                    else if ($style eq 'table') then $tableStyles
                    else if ($style eq 'table-caption') then $tableCaptionStyles
                    else if ($style eq 'table-column') then $tableColumnStyles
                    else if ($style eq 'table-row-group') then $tableRowGroupStyles
                    else if ($style eq 'table-row') then $tableRowStyles
                    else if ($style eq 'table-cell') then $tableCellStyles
                    else $style"/>

      <xsl:variable name="styles" select="distinct-values($selection)"/>

      <xsl:variable name="fo" select="."/>

      <xsl:for-each 
        select="tokenize(string(./processing-instruction('css-styles')), 
                         '&#xA;')">
        <xsl:variable name="propName" select="substring-before(., ':')"/>
        <xsl:variable name="propValue" select="substring-after(., ':')"/>

        <xsl:if test="u:containsPropName($styles, $propName)">
          <xsl:for-each select="$fo">
            <xsl:choose>
              <xsl:when test="$propName eq 'margin-top'">
                <xsl:choose>
                  <xsl:when test="$isInline or 
                                  $propValue eq 'auto' or
                                  $propValue eq 'inherit' or
                                  ends-with($propValue, '%')">
                    <xsl:attribute name="margin-top" 
                                   select="u:asLengthOrPercent($propValue)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="space-before" 
                                   select="u:asLength($propValue)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="$propName eq 'margin-bottom'">
                <xsl:choose>
                  <xsl:when test="$isInline or 
                                  $propValue eq 'auto' or
                                  $propValue eq 'inherit' or
                                  ends-with($propValue, '%')">
                    <xsl:attribute name="margin-bottom" 
                                   select="u:asLengthOrPercent($propValue)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="space-after" 
                                   select="u:asLength($propValue)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="$propName eq 'margin-left'">
                <xsl:choose>
                  <xsl:when test="not($isInline) or 
                                  $propValue eq 'auto' or
                                  $propValue eq 'inherit'">
                    <xsl:attribute name="margin-left" 
                                   select="u:asLengthOrPercent($propValue)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="space-start" 
                                   select="u:asLengthOrPercent($propValue)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="$propName eq 'margin-right'">
                <xsl:choose>
                  <xsl:when test="not($isInline) or 
                                  $propValue eq 'auto' or
                                  $propValue eq 'inherit'">
                    <xsl:attribute name="margin-right" 
                                   select="u:asLengthOrPercent($propValue)"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="space-end" 
                                   select="u:asLengthOrPercent($propValue)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:when test="$propName eq 'padding-top'">
                <xsl:attribute name="padding-top" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'padding-right'">
                <xsl:attribute name="padding-right" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'padding-bottom'">
                <xsl:attribute name="padding-bottom" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'padding-left'">
                <xsl:attribute name="padding-left" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-top-style'">
                <xsl:attribute name="border-top-style" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-right-style'">
                <xsl:attribute name="border-right-style" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-bottom-style'">
                <xsl:attribute name="border-bottom-style" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-left-style'">
                <xsl:attribute name="border-left-style" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-top-width'">
                <xsl:attribute name="border-top-width" 
                               select="u:asLength($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-right-width'">
                <xsl:attribute name="border-right-width" 
                               select="u:asLength($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-bottom-width'">
                <xsl:attribute name="border-bottom-width" 
                               select="u:asLength($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-left-width'">
                <xsl:attribute name="border-left-width" 
                               select="u:asLength($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-top-color'">
                <xsl:attribute name="border-top-color" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-right-color'">
                <xsl:attribute name="border-right-color" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-bottom-color'">
                <xsl:attribute name="border-bottom-color" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'border-left-color'">
                <xsl:attribute name="border-left-color" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'background-color'">
                <xsl:attribute name="background-color" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'background-image'">
                <xsl:attribute name="background-image" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'background-repeat'">
                <xsl:attribute name="background-repeat" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'background-position-horizontal'">
                <xsl:attribute name="background-position-horizontal" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'background-position-vertical'">
                <xsl:attribute name="background-position-vertical" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'color'">
                <xsl:attribute name="color" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'font-family'">
                <xsl:attribute name="font-family" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'font-style'">
                <xsl:attribute name="font-style" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'font-weight'">
                <xsl:attribute name="font-weight" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'font-size'">
                <xsl:attribute name="font-size" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'text-decoration'">
                <xsl:attribute name="text-decoration" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'text-align'">
                <xsl:attribute name="text-align" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'text-indent'">
                <xsl:attribute name="text-indent" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'vertical-align'">
                <xsl:attribute name="vertical-align" select="$propValue"/>
              </xsl:when>
              <xsl:when test="$propName eq 'line-height'">
                <xsl:attribute name="line-height">
                  <xsl:variable name="propValue2" 
                                select="normalize-space($propValue)"/>

                  <xsl:choose>
                    <!-- Here, a bare number does not mean pixels. -->
                    <xsl:when test="number($propValue2) gt 0">
                      <xsl:sequence select="$propValue2"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:sequence select="u:asLengthOrPercent($propValue2)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </xsl:when>
              <xsl:when test="$propName eq 'width'">
                <xsl:attribute name="width" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'height'">
                <xsl:attribute name="height" 
                               select="u:asLengthOrPercent($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 'caption-side'">
                <xsl:attribute name="caption-side" select="$propValue"/>
              </xsl:when> 
              <xsl:when test="$propName eq 
                              'border-separation.inline-progression-direction'">
                <xsl:attribute 
                  name="border-separation.inline-progression-direction" 
                  select="u:asLength($propValue)"/>
              </xsl:when>
              <xsl:when test="$propName eq 
                              'border-separation.block-progression-direction'">
                <xsl:attribute 
                  name="border-separation.block-progression-direction" 
                  select="u:asLength($propValue)"/>
              </xsl:when>
            </xsl:choose>
          </xsl:for-each>
        </xsl:if>
      </xsl:for-each>
    </xsl:if>
  </xsl:template>

  <xsl:function name="u:containsPropName" as="xs:boolean">
    <xsl:param name="styles" as="xs:string*"/>
    <xsl:param name="propName" as="xs:string"/>

    <xsl:variable name="hits" 
                  select="for $style in $styles
                          return
                              if ($propName eq $style or 
                                  starts-with($propName, $style))
                              then $style
                              else ()"/>

    <xsl:sequence select="exists($hits)"/>
  </xsl:function>

  <xsl:function name="u:cssStyle" as="xs:string">
    <xsl:param name="element" as="element()"/>
    <xsl:param name="style" as="xs:string"/>

    <xsl:choose>
      <xsl:when test="$apply-css-styles eq 'yes' and 
                      $element/processing-instruction('css-styles')">
        <xsl:variable name="found" as="xs:string*">
          <xsl:for-each 
            select="tokenize(string($element/processing-instruction('css-styles')), 
                    '&#xA;')">
            <xsl:variable name="propName" select="substring-before(., ':')"/>
            <xsl:variable name="propValue" select="substring-after(., ':')"/>

            <xsl:if test="$style eq $propName">
              <xsl:sequence select="$propValue"/>
            </xsl:if>
          </xsl:for-each>
        </xsl:variable>

        <xsl:sequence select="if (count($found) gt 0) then $found[1] else ''"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:sequence select="''"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>

  <xsl:function name="u:listStyleType" as="xs:string">
    <xsl:param name="element" as="element()"/>

    <xsl:variable name="cssType" 
                  select="u:cssStyle($element, '-list-style-type')"/>

    <xsl:choose>
      <xsl:when test="$cssType ne ''">
        <xsl:choose>
          <xsl:when test="$cssType eq 'lower-alpha' or
                          $cssType eq 'lower-latin'">a</xsl:when>
          <xsl:when test="$cssType eq 'upper-alpha' or
                          $cssType eq 'upper-latin'">A</xsl:when>
          <xsl:when test="$cssType eq 'lower-roman'">i</xsl:when>
          <xsl:when test="$cssType eq 'upper-roman'">I</xsl:when>
          <xsl:when test="$cssType eq 'decimal'">1</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$cssType"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
      <xsl:when test="exists($element/@type)">
        <xsl:sequence select="string($element/@type)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$element/self::html:li">
            <!-- This property is inherited in CSS but has no equivalent in
                 XSL-FO. -->
            <xsl:sequence select="u:listStyleType($element/parent::*)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:sequence select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>

  <xsl:function name="u:listStyleImage" as="xs:string">
    <xsl:param name="element" as="element()"/>

    <xsl:variable name="cssImage" 
                  select="u:cssStyle($element, '-list-style-image')"/>

    <xsl:choose>
      <xsl:when test="$cssImage ne ''">
        <xsl:sequence select="$cssImage"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$element/self::html:li">
            <!-- This property is inherited in CSS but has no equivalent in
                 XSL-FO. -->
            <xsl:sequence select="u:listStyleImage($element/parent::*)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:sequence select="''"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:function>

  <xsl:function name="u:captionSide" as="xs:string">
    <xsl:param name="element" as="element()"/>

    <xsl:variable name="cssCaptionSide" 
                  select="u:cssStyle($element, 'caption-side')"/>

    <xsl:sequence select="if ($cssCaptionSide ne '') 
                          then $cssCaptionSide 
                          else string($element/@align)"/><!--LEGACY-->
  </xsl:function>

</xsl:stylesheet>
