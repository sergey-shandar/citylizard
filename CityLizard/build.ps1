#
# Before running the script: 
#  1. Allow running scripts:
#         powershell Set-ExecutionPolicy RemoteSigned
#  2. Set .NET 4.0 runtime:
#         copy powershell_ise.exe.config C:\Windows\System32\WindowsPowerShell\v1.0\
#  3. Run the script in PowerShell ISE.
#

#
# Settings
#
$company = "CityLizard"

$_7z = "C:\Program Files\7-Zip\7z.exe"
$sandcastle = "C:\Program Files (x86)\Sandcastle\ProductionTools\ChmBuilder.exe"

#
# Directory of this script.
#
$dir = Split-Path -parent $MyInvocation.MyCommand.Definition
[IO.Directory]::SetCurrentDirectory($dir)

#
# Compilation of C# files.
#
$params = 
@{
    Path =
        "CodeDom\Code.cs",
        "Hg\Hg.cs",
        "Build\Build.cs",
        "Policy\Build\Base.cs"
    ReferencedAssemblies = 
        "System.Core", "Microsoft.Build"
}
Add-Type @params

#
# HG root.
#
""
$root = [CityLizard.Hg.Hg]::Root()
"HG root: " + $root

#
# HG summary.
#
""
$summary = [CityLizard.Hg.Hg]::Summary()
$version = [CityLizard.Build.Build]::Version($summary)
"Version: " + $version

#
# Generating AssemblyInfo.cs files for all C# projects.
#
""
"C# projects:"
foreach($f in [CityLizard.Hg.Hg]::Locate())
{
    if([IO.Path]::GetExtension($f) -eq ".csproj")
    {
        "    " + [IO.Path]::GetFileNameWithoutExtension($f)
        $path = Join-Path $root $f
        [CityLizard.Build.Build]::CreateAssemblyInfo($summary, $company, $path)
    }
}

#
# Generating Policy\Base.cs
#
[CityLizard.Policy.Build.Base]::Run()

#
# Building CityLizard.sln
#
""
"Solution: CityLizard"
$path = ""
foreach($f in [CityLizard.Hg.Hg]::Locate())
{
    if([IO.Path]::GetFileName($f) -eq "CityLizard.sln")
    {
        $path = Join-Path $root $f
        [CityLizard.Build.Build]::BuildSolution($path)
        break;
    }
}

$third_party = Join-Path $root "third_party"
$typed_dom = Join-Path $root "CityLizard\TypedDom"
$console = Join-Path $root "CityLizard\Xml\Schema\Console\bin\Debug\CityLizard.Xml.Schema.Console.exe"

#
# Building XHTML 5
#
""
"Building XHTML 5"
$xhtml_xsd = Join-Path $third_party "www.w3.org\MarkUp\SCHEMA\xhtml5.xsd"
$xhtml_dir = Join-Path $typed_dom "www_w3_org._1999.xhtml"
$xhtml_xsd_cs = Join-Path $xhtml_dir "X.xsd.cs"
&$console $xhtml_xsd $xhtml_xsd_cs

#
# Building GraphML 1.1
#
""
"Building GrapnML 1.1"
$graphml_xsd = Join-Path $third_party "graphml.graphdrawing.org\xmlns\1.1\graphml.xsd"
$graphml_dir = Join-Path $typed_dom "graphml_graphdrawing_org.xmlns"
$graphml_xsd_cs = Join-Path $graphml_dir "X.xsd.cs"
&$console $graphml_xsd $graphml_xsd_cs

#
# Building SVG 1.1
#
""
"Building SVG 1.1"
$svg_xsd = Join-Path $third_party "www.w3.org\TR\2002\WD-SVG11-20020108\SVG.xsd"
$svg_dir = Join-Path $typed_dom "www_w3_org._2000.svg"
$svg_xsd_cs = Join-Path $svg_dir "X.xsd.cs"
&$console $svg_xsd $svg_xsd_cs

#
# Building NuGet
#
""
"Building NuGet"
$nuget_xsd = Join-Path $third_party "nuget.codeplex.com\nuspec.xsd"
$nuget_dir = Join-Path $typed_dom "schemas_microsoft_com.packaging._2010._07.nuspec_xsd"
$nuget_xsd_cs = Join-Path $nuget_dir "X.xsd.cs"
&$console $nuget_xsd $nuget_xsd_cs

#
# Building WiX
#
""
"Building WiX"
$wix_xsd = Join-Path $third_party "wix.codeplex.com\wix.xsd"
$wix_dir = Join-Path $typed_dom "schemas_microsoft_com.wix.2006.wi"
$wix_xsd_cs = Join-Path $nuget_dir "X.xsd.cs"
&$console $wix_xsd $wix_xsd_cs

#
# Building CityLizard.TypedDom.sln
#
""
"Solution: CityLizard.TypedDom"
$typed_dom = ""
$typed_dom_dir = ""
$sl_typed_dom = ""
foreach($f in [CityLizard.Hg.Hg]::Locate())
{
    if([IO.Path]::GetFileName($f) -eq "CityLizard.TypedDom.sln")
    {
        $typed_dom = Join-Path $root $f
        $typed_dom_dir = Split-Path -parent $typed_dom        
        # $sl_typed_dom = Join-Path $typed_dom_dir "SL.CityLizard.TypedDom.sln"
        # [CityLizard.Build.Build]::BuildSolution($typed_dom)
        # [CityLizard.Build.Build]::BuildSolution($sl_typed_dom)        
        C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe $typed_dom
        break;
    }
}

#
# Sandcastle
#
# ""
# "Sandcastle"
# $html = Join-Path $root "html"
# &$sandcastle "/html:" $html "/project:" $path

#
# Zipping
#
""
"Zip:"

$zipName = "CityLizard." + $version + ".zip"

$zip = Join-Path $root $zipName

$xml = Join-Path $root "CityLizard\Xml\bin\Debug\CityLizard.Xml.dll"
$sl_xml = Join-Path $root "CityLizard\Xml\Silverlight\bin\Debug\CityLizard.Xml.dll"
$xml_35 = Join-Path $root "CityLizard\Xml\3.5\bin\Debug\CityLizard.Xml.dll"

$schema = Join-Path $root "CityLizard\Xml\Schema\bin\Debug\CityLizard.Xml.Schema.dll"
$codedom = Join-Path $root "CityLizard\CodeDom\bin\Debug\CityLizard.CodeDom.dll"
$console = Join-Path $root "CityLizard\Xml\Schema\Console\bin\Debug\CityLizard.Xml.Schema.Console.exe"

$xhtml = Join-Path $xhtml_dir "bin\Debug\www_w3_org._1999.xhtml.dll"
$sl_xhtml = Join-Path $xhtml_dir "Silverlight\bin\Debug\www_w3_org._1999.xhtml.dll"
$xhtml_35 = Join-Path $xhtml_dir "3.5\bin\Debug\www_w3_org._1999.xhtml.dll"

$graphml = Join-Path $graphml_dir "bin\Debug\graphml_graphdrawing_org.xmlns.dll"
$sl_graphml = Join-Path $graphml_dir "Silverlight\bin\Debug\graphml_graphdrawing_org.xmlns.dll"
$graphml_35 = Join-Path $graphml_dir "3.5\bin\Debug\graphml_graphdrawing_org.xmlns.dll"

$svg = Join-Path $svg_dir "bin\Debug\www_w3_org._2000.svg.dll"
$sl_svg = Join-Path $svg_dir "Silverlight\bin\Debug\www_w3_org._2000.svg.dll"
$svg_35 = Join-Path $svg_dir "3.5\bin\Debug\www_w3_org._2000.svg.dll"

$nuget = Join-Path $nuget_dir "bin\Debug\schemas_microsoft_com.packaging._2010._07.nuspec_xsd.dll"
$sl_nuget = Join-Path $nuget_dir "Silverlight\bin\Debug\schemas_microsoft_com.packaging._2010._07.nuspec_xsd.dll"
$nuget_35 = Join-Path $nuget_dir "3.5\bin\Debug\schemas_microsoft_com.packaging._2010._07.nuspec_xsd.dll"

$license = Join-Path $root "CityLizard\license.txt"

$lib = Join-Path $root "lib"
mkdir $lib

$lib_net4 = Join-Path $lib "NETFramework4.0"
mkdir $lib_net4

$lib_net35 = Join-Path $lib "NETFramework3.5"
mkdir $lib_net35

$lib_sl = Join-Path $lib "Silverlight"
mkdir $lib_sl

copy $xml $lib_net4
copy $xml_35 $lib_net35
copy $sl_xml $lib_sl

copy $schema $lib_net4
copy $codedom $lib_net4
copy $console $lib_net4

copy $xhtml $lib_net4
copy $xhtml_35 $lib_net35
copy $sl_xhtml $lib_sl

copy $graphml $lib_net4
copy $graphml_35 $lib_net35
copy $sl_graphml $lib_sl

copy $svg $lib_net4
copy $svg_35 $lib_net35
copy $sl_svg $lib_sl

copy $nuget $lib_net4
copy $nuget_35 $lib_net35
copy $sl_nuget $lib_sl

# &$_7z "a" $zip $xml $xhtml $graphml $svg $schema $codedom $console $license $sl_xml $sl_xhtml, $sl_graphml, $sl_svg
&$_7z "a" $zip $lib $license
