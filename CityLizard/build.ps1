#
# Before running the script, run:
# > Set-ExecutionPolicy RemoteSigned
#
Add-Type -Path .\Hg\Hg.cs
$version = [CityLizard.Hg.Hg]::Summary().Version()
"version: " + $version
$locate = [CityLizard.Hg.Hg]::Locate()
$locate
