#
# Before running the script, 
#  1. Execute in PowerShell:
#     Set-ExecutionPolicy RemoteSigned
#  2. Copy powershell_ise.exe.config C:\Windows\System32\WindowsPowerShell\v1.0\
#

$dir = Split-Path -parent $MyInvocation.MyCommand.Definition
$path = Join-Path $dir Hg\Hg.cs

"path: " + $path

[System.IO.Directory]::SetCurrentDirectory($dir)

Add-Type -Path $path

"root: " + [CityLizard.Hg.Hg]::Root()

$version = [CityLizard.Hg.Hg]::Summary()
"version: " + $version
$locate = [CityLizard.Hg.Hg]::Locate()
$locate
