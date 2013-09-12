setlocal

set LIB=boost.%1
set NUSPEC=%LIB%.nuspec
set VERSION=1.54.0.23

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
@echo ^<description^>%LIB%^</description^> >> %NUSPEC%
@echo ^<dependencies^> >> %NUSPEC%
FOR %%d IN (%*) DO (
IF not %%d == %1 (
@echo ^<dependency id="boost.%%d" version="[%VERSION%]" /^> >> %NUSPEC%
)
)
@echo ^</dependencies^> >> %NUSPEC%
@echo ^</metadata^> >> %NUSPEC%
@echo ^<files^> >> %NUSPEC%
@echo ^<file src="..\..\..\..\Downloads\boost_1_54_0\boost\%1*\**\*.*" target="lib\native\include\boost\"/^> >> %NUSPEC%
@echo ^<file src="..\..\..\..\Downloads\boost_1_54_0\boost\%1.hpp*" target="lib\native\include\boost\"/^> >> %NUSPEC%
@echo ^<file src="..\..\..\..\Downloads\boost_1_54_0\boost\%1_fwd.hpp*" target="lib\native\include\boost\"/^> >> %NUSPEC%
@echo ^<file src="..\..\..\..\Downloads\boost_1_54_0\stage_x86\lib\*%1.*" target="lib\native\stage_x86\"/^> >> %NUSPEC%
@echo ^<file src="..\..\..\..\Downloads\boost_1_54_0\stage_x86_64\lib\*%1.*" target="lib\native\stage_x86_64\"/^> >> %NUSPEC%
@echo ^<file src="boost.%1.targets" target="build\native\"/^> >> %NUSPEC%
@echo ^</files^> >> %NUSPEC%
@echo ^</package^> >> %NUSPEC%

copy ..\boost.targets boost.%1.targets

nuget.exe pack %NUSPEC%

copy %LIB%.%VERSION%.nupkg c:\nuget

endlocal