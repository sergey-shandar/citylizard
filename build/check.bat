setlocal
set PATH=%PATH%;%ProgramFiles(x86)%\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools;

sn -T net35-client\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T net35-client\CityLizard.Fsm\bin\Release\CityLizard.Fsm.dll
sn -T net35-client\CityLizard.Meta\bin\Release\CityLizard.Meta.dll

sn -T net40-client\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T net40-client\CityLizard.Fsm\bin\Release\CityLizard.Fsm.dll
sn -T net40-client\CityLizard.Meta\bin\Release\CityLizard.Meta.dll

sn -T sl4\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T sl4\CityLizard.Fsm\bin\Release\CityLizard.Fsm.dll

sn -T sl5\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T sl5\CityLizard.Fsm\bin\Release\CityLizard.Fsm.dll

sn -T netcore45\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T netcore45\CityLizard.Fsm\bin\Release\CityLizard.Fsm.dll

sn -T psm\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T psm\CityLizard.Fsm\bin\Release\CityLizard.Fsm.dll

sn -T monoandroid\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T monoandroid\CityLizard.Fsm\bin\Release\CityLizard.Fsm.dll

sn -T monotouch\CityLizard.Fsm\bin\Release\CityLizard.Core.dll
sn -T monotouch\CityLizard.Fsm\bin\Release\CityLizard.Fsm.dll

sn -T wp8\CityLizard.Core\bin\Release\CityLizard.Core.dll
sn -T wp8\CityLizard.Fsm\bin\Release\CityLizard.Fsm.dll

sn -T sl4-wp71\CityLizard.Core\bin\Release\CityLizard.Core.dll

endlocal