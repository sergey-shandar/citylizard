<?xml version="1.0" encoding="UTF-8"?>

<!-- =====================================================================
     XSLT 2 stylesheets for XHTML 1.0, 1.1, 5.0.
     Stylesheet parameters.

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
                exclude-result-prefixes="xs"
                version="2.0">

  <xsl:param name="apply-css-styles" select="'yes'"/>

  <xsl:param name="base-font-size" select="'10pt'"/>
  <xsl:param name="font-family" select="'serif'"/>
  <xsl:param name="justified" select="'no'"/>
  <xsl:param name="hyphenate" select="'no'"/>

  <!-- Few glyphs are supported by the combination of XSL-FO
       processor/built-in PDF fonts. These are supported by FOP and
       XEP. -->
  <xsl:param name="ul-li-bullets" select="'&#x2022; &#x2013;'"/>
  <xsl:param name="ulLiBullets" select="tokenize($ul-li-bullets, '\s+')" />

  <xsl:param name="show-external-links" select="'no'"/>
  <xsl:param name="external-href-before" select="' ['"/>
  <xsl:param name="external-href-after" select="']'"/>

  <xsl:param name="show-xref-page" select="'no'"/>
  <xsl:param name="page-ref-before" select="' ['"/>
  <xsl:param name="page-ref-after" select="']'"/>

  <!-- A string possibly containing a mix of text and one or more of 
       the following variables: 
       {{document-title}}, {{current-heading}},
       {{page-number}}, {{odd-page-number}}, {{even-page-number}}, 
       {{page-count}}, {{break}}, {{image(location)}}, {{url(location)}}. -->
  <xsl:param name="header-left" select="''"/>
  <xsl:param name="header-center" select="'{{document-title}}'"/>
  <xsl:param name="header-right" select="''"/>

  <xsl:param name="footer-left" select="'{{even-page-number}}'"/>
  <xsl:param name="footer-center" select="''"/>
  <xsl:param name="footer-right" select="'{{odd-page-number}}'"/>

  <xsl:param name="header-separator" select="'yes'"/>
  <xsl:param name="footer-separator" select="'yes'"/>

  <xsl:param name="header-left-width" select="'2'"/>
  <xsl:param name="header-center-width" select="'6'"/>
  <xsl:param name="header-right-width" select="'2'"/>

  <xsl:param name="footer-left-width"  select="'2'"/>
  <xsl:param name="footer-center-width" select="'6'"/>
  <xsl:param name="footer-right-width" select="'2'"/>

  <xsl:param name="set-outline-level" select="'yes'"/>

  <!-- Page dimension and orientation ==================================== -->

  <xsl:param name="paper-type" select="'A4'"/>

  <xsl:param name="page-orientation" select="'portrait'"/>

  <xsl:param name="page-width"
             select="if ($page-orientation eq 'portrait') 
                     then $paper-width
                     else $paper-height"/>
  <xsl:param name="page-height" 
             select="if ($page-orientation eq 'portrait') 
                     then $paper-height 
                     else $paper-width"/>

  <xsl:param name="paper-width" as="xs:string">
    <xsl:variable name="paperType" select="lower-case($paper-type)"/>
    <xsl:choose>
      <xsl:when test="$paperType eq 'letter'">8.5in</xsl:when>
      <xsl:when test="$paperType eq 'legal'">8.5in</xsl:when>
      <xsl:when test="$paperType eq 'ledger'">17in</xsl:when>
      <xsl:when test="$paperType eq 'tabloid'">11in</xsl:when>
      <xsl:when test="$paperType eq 'a0'">841mm</xsl:when>
      <xsl:when test="$paperType eq 'a1'">594mm</xsl:when>
      <xsl:when test="$paperType eq 'a2'">420mm</xsl:when>
      <xsl:when test="$paperType eq 'a3'">297mm</xsl:when>
      <xsl:when test="$paperType eq 'a4'">210mm</xsl:when>
      <xsl:when test="$paperType eq 'a5'">148mm</xsl:when>
      <xsl:when test="$paperType eq 'a6'">105mm</xsl:when>
      <xsl:when test="$paperType eq 'a7'">74mm</xsl:when>
      <xsl:when test="$paperType eq 'a8'">52mm</xsl:when>
      <xsl:when test="$paperType eq 'a9'">37mm</xsl:when>
      <xsl:when test="$paperType eq 'a10'">26mm</xsl:when>
      <xsl:when test="$paperType eq 'b0'">1000mm</xsl:when>
      <xsl:when test="$paperType eq 'b1'">707mm</xsl:when>
      <xsl:when test="$paperType eq 'b2'">500mm</xsl:when>
      <xsl:when test="$paperType eq 'b3'">353mm</xsl:when>
      <xsl:when test="$paperType eq 'b4'">250mm</xsl:when>
      <xsl:when test="$paperType eq 'b5'">176mm</xsl:when>
      <xsl:when test="$paperType eq 'b6'">125mm</xsl:when>
      <xsl:when test="$paperType eq 'b7'">88mm</xsl:when>
      <xsl:when test="$paperType eq 'b8'">62mm</xsl:when>
      <xsl:when test="$paperType eq 'b9'">44mm</xsl:when>
      <xsl:when test="$paperType eq 'b10'">31mm</xsl:when>
      <xsl:when test="$paperType eq 'c0'">917mm</xsl:when>
      <xsl:when test="$paperType eq 'c1'">648mm</xsl:when>
      <xsl:when test="$paperType eq 'c2'">458mm</xsl:when>
      <xsl:when test="$paperType eq 'c3'">324mm</xsl:when>
      <xsl:when test="$paperType eq 'c4'">229mm</xsl:when>
      <xsl:when test="$paperType eq 'c5'">162mm</xsl:when>
      <xsl:when test="$paperType eq 'c6'">114mm</xsl:when>
      <xsl:when test="$paperType eq 'c7'">81mm</xsl:when>
      <xsl:when test="$paperType eq 'c8'">57mm</xsl:when>
      <xsl:when test="$paperType eq 'c9'">40mm</xsl:when>
      <xsl:when test="$paperType eq 'c10'">28mm</xsl:when>
      <xsl:otherwise>210mm</xsl:otherwise>
    </xsl:choose>
  </xsl:param>

  <xsl:param name="paper-height" as="xs:string">
    <xsl:variable name="paperType" select="lower-case($paper-type)"/>
    <xsl:choose>
      <xsl:when test="$paperType eq 'letter'">11in</xsl:when>
      <xsl:when test="$paperType eq 'legal'">14in</xsl:when>
      <xsl:when test="$paperType eq 'ledger'">11in</xsl:when>
      <xsl:when test="$paperType eq 'tabloid'">17in</xsl:when>
      <xsl:when test="$paperType eq 'a0'">1189mm</xsl:when>
      <xsl:when test="$paperType eq 'a1'">841mm</xsl:when>
      <xsl:when test="$paperType eq 'a2'">594mm</xsl:when>
      <xsl:when test="$paperType eq 'a3'">420mm</xsl:when>
      <xsl:when test="$paperType eq 'a4'">297mm</xsl:when>
      <xsl:when test="$paperType eq 'a5'">210mm</xsl:when>
      <xsl:when test="$paperType eq 'a6'">148mm</xsl:when>
      <xsl:when test="$paperType eq 'a7'">105mm</xsl:when>
      <xsl:when test="$paperType eq 'a8'">74mm</xsl:when>
      <xsl:when test="$paperType eq 'a9'">52mm</xsl:when>
      <xsl:when test="$paperType eq 'a10'">37mm</xsl:when>
      <xsl:when test="$paperType eq 'b0'">1414mm</xsl:when>
      <xsl:when test="$paperType eq 'b1'">1000mm</xsl:when>
      <xsl:when test="$paperType eq 'b2'">707mm</xsl:when>
      <xsl:when test="$paperType eq 'b3'">500mm</xsl:when>
      <xsl:when test="$paperType eq 'b4'">353mm</xsl:when>
      <xsl:when test="$paperType eq 'b5'">250mm</xsl:when>
      <xsl:when test="$paperType eq 'b6'">176mm</xsl:when>
      <xsl:when test="$paperType eq 'b7'">125mm</xsl:when>
      <xsl:when test="$paperType eq 'b8'">88mm</xsl:when>
      <xsl:when test="$paperType eq 'b9'">62mm</xsl:when>
      <xsl:when test="$paperType eq 'b10'">44mm</xsl:when>
      <xsl:when test="$paperType eq 'c0'">1297mm</xsl:when>
      <xsl:when test="$paperType eq 'c1'">917mm</xsl:when>
      <xsl:when test="$paperType eq 'c2'">648mm</xsl:when>
      <xsl:when test="$paperType eq 'c3'">458mm</xsl:when>
      <xsl:when test="$paperType eq 'c4'">324mm</xsl:when>
      <xsl:when test="$paperType eq 'c5'">229mm</xsl:when>
      <xsl:when test="$paperType eq 'c6'">162mm</xsl:when>
      <xsl:when test="$paperType eq 'c7'">114mm</xsl:when>
      <xsl:when test="$paperType eq 'c8'">81mm</xsl:when>
      <xsl:when test="$paperType eq 'c9'">57mm</xsl:when>
      <xsl:when test="$paperType eq 'c10'">40mm</xsl:when>
      <xsl:otherwise>297mm</xsl:otherwise>
    </xsl:choose>
  </xsl:param>

  <xsl:param name="two-sided" select="'no'"/>

  <xsl:param name="page-top-margin" select="'0.5in'"/>
  <xsl:param name="page-bottom-margin" select="'0.5in'"/>
  <xsl:param name="page-inner-margin"
             select="if ($two-sided eq 'yes') then '1.25in' else '1in'"/>
  <xsl:param name="page-outer-margin"
             select="if ($two-sided eq 'yes') then '0.75in' else '1in'"/>

  <xsl:param name="body-top-margin" select="'0.5in'"/>
  <xsl:param name="body-bottom-margin" select="'0.5in'"/>

  <xsl:param name="header-height" select="'0.4in'"/>
  <xsl:param name="footer-height" select="'0.4in'"/>

  <!-- Advanced parameters =============================================== -->

  <!-- Specifies the ID of the element to be formatted.
       By default (empty string), it is the whole document. -->
  <xsl:param name="root-id" select="''"/>

  <!-- Prepended to the src attribute of the generated fo:external-graphic,
       unless the src of the XHTML img is absolute. 
       This value must be empty or must end with a '/'. -->
  <xsl:param name="img-src-path" select="''"/>

  <xsl:param name="resolve-img-src" select="'yes'"/>

  <xsl:param name="resolve-a-href" select="'no'"/>

  <!-- Screen resolution in DPI. Used to convert px to pt. -->
  <xsl:param name="screen-resolution" select="96.0" />

  <!-- Parameters set by the client code, not by the end user ============ -->

  <xsl:param name="outputFormat" select="''"/>

  <!-- If the name of the system property is in the null namespace, Saxon
       returns the value of the Java system property whose name matches the
       local name. -->

  <xsl:param name="foProcessor"
    select="if ($outputFormat eq 'pdf' or $outputFormat eq 'ps') then
                if (contains(system-property('XSL_FO_PROCESSORS'),
                                             'AHF')) 
                then 'AHF'
                else
                    if (contains(system-property('XSL_FO_PROCESSORS'),
                                                 'XEP')) 
                    then 'XEP'
                    else
                        if (contains(system-property('XSL_FO_PROCESSORS'),
                                                     'FOP')) 
                        then 'FOP'
                        else ''
            else
                if ($outputFormat eq 'rtf' or 
                    $outputFormat eq 'wml' or 
                    $outputFormat eq 'docx' or 
                    $outputFormat eq 'odt') 
                then
                    if (contains(system-property('XSL_FO_PROCESSORS'),
                                                 'XFC')) 
                    then 'XFC'
                    else ''
                else ''"/>

</xsl:stylesheet>
