setlocal

set LIB=boost_%1
set NUSPEC=%LIB%.nuspec

@echo ^<?xml version="1.0" encoding="utf-8"?^> > %NUSPEC%
@echo ^<package xmlns="http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"^> >> %NUSPEC%
@echo ^<metadata^> >> %NUSPEC%
@echo ^<id^>%LIB%^</id^> >> %NUSPEC%
@echo ^<version^>%VERSION%^</version^> >> %NUSPEC%
@echo ^<authors^>Sergey Shandar^</authors^> >> %NUSPEC%
@echo ^<owners^>Sergey Shandar^</owners^> >> %NUSPEC%
@echo ^<licenseUrl^>http://www.boost.org/LICENSE_1_0.txt^</licenseUrl^> >> %NUSPEC%
@echo ^<projectUrl^>http://boost.org/^</projectUrl^> >> %NUSPEC%
@echo ^<requireLicenseAcceptance^>false^</requireLicenseAcceptance^> >> %NUSPEC%
@echo ^<description^>Precompiled %LIB% libraries for Visual Studio 2012. Platforms: Win32 and x64.^</description^> >> %NUSPEC%
@echo ^<dependencies^> >> %NUSPEC%
@echo ^<dependency id="boost" version="[%VERSION%]" /^> >> %NUSPEC%
@echo ^</dependencies^> >> %NUSPEC%
@echo ^</metadata^> >> %NUSPEC%
@echo ^<files^> >> %NUSPEC%
@echo ^<file src="..\..\..\..\Downloads\boost_1_54_0\stage_x86\lib\*%LIB%-vc110*.*" target="lib\native\stage_x86\"/^> >> %NUSPEC%
@echo ^<file src="..\..\..\..\Downloads\boost_1_54_0\stage_x86_64\lib\*%LIB%-vc110*.*" target="lib\native\stage_x86_64\"/^> >> %NUSPEC%
@echo ^<file src="%LIB%.targets" target="build\native\"/^> >> %NUSPEC%
@echo ^</files^> >> %NUSPEC%
@echo ^</package^> >> %NUSPEC%

copy ..\boost.lib.targets %LIB%.targets

nuget.exe pack %NUSPEC%

rem copy %LIB%.%VERSION%.nupkg c:\nuget
nuget.exe push %LIB%.%VERSION%.nupkg

endlocal