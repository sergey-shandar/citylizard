setlocal
set PATH=%PATH%;%WINDIR%\Microsoft.NET\Framework\v4.0.30319;C:\programs;%ProgramFiles(x86)%\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools;%ProgramFiles%\7-Zip;

for /F %%I in (version.txt) do set VERSION=%%I

call assembly_info.bat CityLizard.Core
call assembly_info.bat CityLizard.Fsm
call assembly_info.bat CityLizard.Meta

call platform_build.bat net35-client
call platform_build.bat net40-client
call platform_build.bat sl4
call platform_build.bat sl5
call platform_build.bat netcore45
call platform_build.bat wp8
call platform_build.bat sl4-wp71
call platform_build.bat psm
rem call platform_build.bat monoandroid
rem call platform_build.bat monotouch
rem call platfrom_build.bat monomac

rem sn -R net35-client\CityLizard.Core\bin\Release\CityLizard.Core.dll keypair.snk

@echo ^<#@ template debug="false" hostspecific="false" language="C#" #^> > CityLizard.Meta.tt.txt
@echo ^<#@ assembly name="System.Core" #^> >> CityLizard.Meta.tt.txt
@echo ^<#@ assembly name="$(SolutionDir)\packages\CityLizard.%VERSION%\lib\net40-client\CityLizard.Meta.dll" #^> >> CityLizard.Meta.tt.txt

nuget pack CityLizard.nuspec -Version %VERSION%

rem rmdir /S /Q lib

rem xcopy ..\platforms\psm\CityLizard.Fsm\bin\Release\*.dll lib\psm\

del CityLizard.psm.%VERSION%.zip
7z a CityLizard.psm.%VERSION%.zip ..\platforms\psm\CityLizard.Fsm\bin\Release\*.dll

endlocal
