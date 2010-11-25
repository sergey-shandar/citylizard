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
        "CodeDom\Extension\AttributeDeclarationCollectionExtension.cs",
        "CodeDom\Extension\NamespaceCollectionExtension.cs",
        "Hg\Hg.cs",
        "Build\Build.cs"
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
# Building XHTML 1.1
#
""
"Building xhtml11.xsd.cs"
$xhtml11_xsd = Join-Path $third_party "www.w3.org\MarkUp\SCHEMA\xhtml11.xsd"
$xhtml11_xsd_cs = Join-Path $typed_dom "www_w3_org._1999.xhtml\X.xsd.cs"
&$console $xhtml11_xsd $xhtml11_xsd_cs

#
# Building GraphML 1.1
#
""
"Building graphml.xsd.cs"
$graphml_xsd = Join-Path $third_party "graphml.graphdrawing.org\xmlns\1.1\graphml.xsd"
$graphml_xsd_cs = Join-Path $typed_dom "graphml_graphdrawing_org.xmlns\X.xsd.cs"
&$console $graphml_xsd $graphml_xsd_cs

#
# Building SVG 1.1
#
""
"Building svg.xsd.cs"
$svg_xsd = Join-Path $third_party "www.w3.org\TR\2002\WD-SVG11-20020108\SVG.xsd"
$svg_xsd_cs = Join-Path $typed_dom "www_w3_org._2000.svg\X.xsd.cs"
&$console $svg_xsd $svg_xsd_cs

#
# Building CityLizard.TypedDom.sln
#
""
"Solution: CityLizard.TypedDom"
$typed_dom = ""
foreach($f in [CityLizard.Hg.Hg]::Locate())
{
    if([IO.Path]::GetFileName($f) -eq "CityLizard.TypedDom.sln")
    {
        $typed_dom = Join-Path $root $f
        [CityLizard.Build.Build]::BuildSolution($typed_dom)
        break;
    }
}
$typed_dom_dir = Split-Path -parent $typed_dom

#
# Building CityLizard.XHtml.sln
#
""
"Solution: CityLizard.XHtml"
$xhtml = ""
foreach($f in [CityLizard.Hg.Hg]::Locate())
{
    if([IO.Path]::GetFileName($f) -eq "CityLizard.XHtml.sln")
    {
        $xhtml = Join-Path $root $f
        [CityLizard.Build.Build]::BuildSolution($xhtml)
        break;
    }
}
$xhtmlDir = Split-Path -parent $xhtml

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
$zipName = "CityLizard.XHtml." + $version + ".zip"
$zip = Join-Path $root $zipName
$xhtml = Join-Path $xhtmlDir "bin\Debug\*.dll"
$graphml = Join-Path $root "CityLizard\GraphML\bin\Debug\CityLizard.GraphML.dll"
$license = Join-Path $root "CityLizard\license.txt"
&$_7z "a" $zip $dll $graphml $license
