setlocal
set PATH=%PATH%C:\Windows\Microsoft.NET\Framework\v4.0.30319;C:\programs;C:\Program Files\7-Zip

for /F %%I in (version.txt) do set VERSION=%%I

echo using System.Reflection; > CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyCompany("CityLizard")] >> CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyCopyright("Copyright © CityLizard 2013")] >> CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyVersion("%VERSION%")] >> CityLizard.AssemblyInfo.cs
echo [assembly: AssemblyFileVersion("%VERSION%")] >> CityLizard.AssemblyInfo.cs

msbuild desktop.sln
msbuild web.sln
msbuild windows8.sln
msbuild wp.sln
msbuild psm.sln /p:Configuration=Release

mkdir lib\native\
echo > lib\native\_._
nuget pack CityLizard.nuspec -Version %VERSION%

rmdir /S /Q lib

xcopy CityLizard.Meta\bin\net40-client\*.dll lib\net40-client\
xcopy CityLizard.Meta\bin\net35-client\*.dll lib\net35-client\
xcopy CityLizard.Fsm\bin\sl4\CityLizard.*.dll lib\sl4\
xcopy CityLizard.Fsm\bin\sl5\CityLizard.*.dll lib\sl5\
xcopy CityLizard.Fsm\bin\windows8\*.dll lib\netcore45\
xcopy CityLizard.Fsm\bin\wp8\*.dll lib\wp8\
xcopy CityLizard.Fsm\bin\Release.psm\*.dll lib\psm\
xcopy CityLizard.Core\bin\sl4-wp71\*.dll lib\sl4-wp71\ 

del CityLizard.%VERSION%.zip
7z a CityLizard.%VERSION%.zip lib

endlocal
