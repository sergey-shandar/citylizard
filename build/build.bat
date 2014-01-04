setlocal
set PATH=%PATH%;%WINDIR%\Microsoft.NET\Framework\v4.0.30319;C:\programs;%ProgramFiles(x86)%\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools;%ProgramFiles%\7-Zip;

for /F %%I in (version.txt) do set VERSION=%%I

call assembly_info.bat CityLizard.Core
call assembly_info.bat CityLizard.Meta

call platform_build.bat net403-client
call platform_build.bat portable-net403+sl5+wp8+win8+monotouch+monoandroid
call platform_build.bat psm

rem sn -R net35-client\CityLizard.Core\bin\Release\CityLizard.Core.dll keypair.snk

@echo ^<#@ template debug="false" hostspecific="true" language="C#" #^> > CityLizard.Meta.tt.txt
@echo ^<#@ assembly name="System.Core" #^> >> CityLizard.Meta.tt.txt
@echo ^<#@ assembly name="$(SolutionDir)\packages\CityLizard.%VERSION%\lib\net403-client\CityLizard.Meta.dll" #^> >> CityLizard.Meta.tt.txt
@echo ^<#@ import namespace="System.IO" #^> >> CityLizard.Meta.tt.txt
@echo ^<#@ import namespace="CityLizard.Xml.Schema" #^> >> CityLizard.Meta.tt.txt

nuget pack CityLizard.nuspec -Version %VERSION%

rem rmdir /S /Q lib

rem xcopy ..\platforms\psm\CityLizard.Fsm\bin\Release\*.dll lib\psm\

del CityLizard.psm.%VERSION%.zip
7z a CityLizard.psm.%VERSION%.zip ..\platforms\psm\CityLizard.Core\bin\Release\*.dll

endlocal
