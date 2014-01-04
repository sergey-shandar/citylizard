setlocal
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools;

sn -T ..\csharp\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T ..\csharp\CityLizard.Meta\bin\Release\CityLizard.Meta.dll
sn -T ..\csharp\psm\CityLizard.Core\bin\Release\CityLizard.Core.dll

endlocal