setlocal
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools;

sn -T portable-net403+sl5+wp8+win8+monotouch+monoandroid\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T net35-client\CityLizard.Meta\bin\Release\CityLizard.Meta.dll

endlocal