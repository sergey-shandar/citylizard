setlocal
set PATH=%PATH%C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\programs;

set VERSION=2.1.6.0

echo using System.Reflection; > CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyCompany("CityLizard")] >> CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyCopyright("Copyright © CityLizard 2013")] >> CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyVersion("%VERSION%")] >> CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyFileVersion("%VERSION%")] >> CityLizard.AssemblyInfo.cs

msbuild desktop.sln /p:Configuration=Release
msbuild web.sln /p:Configuration=Release
msbuild windows8.sln /p:Configuration=Release
msbuild wp.sln /p:Configuration=Release
msbuild psm.sln /p:Configuration=Release

nuget pack CityLizard.nuspec -Version %VERSION%

endlocal