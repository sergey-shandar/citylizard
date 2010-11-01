#
# Before running the script, 
#  1. Execute in PowerShell:
#     Set-ExecutionPolicy RemoteSigned
#  2. Copy powershell_ise.exe.config C:\Windows\System32\WindowsPowerShell\v1.0\
#

$company = "CityLizard"

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
        "CodeDom\Extension\AttributeDeclarationCollection.cs",
        "CodeDom\Extension\NamespaceCollectionExtension.cs",
        "Hg\Hg.cs"
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
$version = $summary.Version()
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
        $summary.CreateAssemblyInfo($company, $path)
    }
}

#
# Building CityLizard.sln
#
""
"Solution: CityLizard"
foreach($f in [CityLizard.Hg.Hg]::Locate())
{
    if([IO.Path]::GetFileName($f) -eq "CityLizard.sln")
    {
        $path = Join-Path $root $f
        [CityLizard.Hg.Hg]::BuildSolution($path)
        break;
    }
}

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
        [CityLizard.Hg.Hg]::BuildSolution($path)
        break;
    }
}
$xhtmlDir = Split-Path -parent $xhtml

#
# Zipping
#
""
"Zip:"
$zipName = "CityLizard.XHtml." + $version + ".zip"
$zip = Join-Path $root $zipName
$dll = Join-Path $xhtmlDir "bin\Debug\*.dll"
$license = Join-Path $root "CityLizard\license.txt"
&"C:\Program Files\7-Zip\7z.exe" "a" $zip $dll $license


