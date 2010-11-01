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
"dir: " + $dir
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
    ReferencedAssemblies = "System.Core"
}
Add-Type @params

#
# Mercurial root.
#
$root = [CityLizard.Hg.Hg]::Root()
"root: " + $root

#
# Mercurial summary.
#
$summary = [CityLizard.Hg.Hg]::Summary()
"version: " + $summary.Version()

#
# Generating AssemblyInfo.cs files for all C# projects.
#
""
"projects:"
foreach($f in [CityLizard.Hg.Hg]::Locate())
{
    if([IO.Path]::GetExtension($f) -eq ".csproj")
    {
        "    " + $f
        $path = Join-Path $root $f
        $summary.CreateAssemblyInfo($company, $path)
    }
}
