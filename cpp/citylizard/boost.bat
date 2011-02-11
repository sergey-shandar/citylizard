setlocal

cd C:\Users\Sergey\Downloads\boost_1_45_0\

rem set VSINSTALLDIR=C:\Program Files (x86)\Microsoft Visual Studio 10.0\

rem set PATH=%PATH%;%VSINSTALLDIR%Common7\IDE\
rem set VSBIN=%VSINSTALLDIR%VC\bin\

setlocal
rem set PATH=%PATH%;%VSBIN%
bjam msvc architecture=x86 stage --stagedir=stage_x86
endlocal

setlocal
rem set PATH=%PATH%;%VSBIN%x86_amd64\
call "C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\SetEnv.Cmd" /x64
bjam msvc architecture=x86 address-model=64 stage --stagedir=stage_x86_64
endlocal

setlocal
rem set PATH=%PATH%;%VSBIN%x86_ia64\
call "C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\SetEnv.Cmd" /ia64
bjam msvc architecture=ia64 stage --stagedir=stage_ia64
endlocal

endlocal