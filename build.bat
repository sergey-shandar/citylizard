setlocal
set PATH=%PATH%C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\programs;

set VERSION=2.1.4.0

echo using System.Reflection; > CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyCompany("CityLizard")] >> CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyCopyright("Copyright © CityLizard 2013")] >> CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyVersion("%VERSION%")]" >> CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyFileVersion("%VERSION%")] >> CityLizard.AssemblyInfo.cs

msbuild desktop.sln
msbuild web.sln
msbuild windows8.sln
msbuild wp.sln
msbuild psm.sln

nuget pack CityLizard.nuspec -Version %VERSION%

endlocal