cd C:\Users\Sergey\Downloads\boost_1_54_0\

call bootstrap.bat

setlocal
bjam msvc architecture=x86 stage --stagedir=stage_x86
endlocal

setlocal
rem call "C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\SetEnv.Cmd" /x64
bjam msvc architecture=x86 address-model=64 stage --stagedir=stage_x86_64
endlocal

rem setlocal
rem call "C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\SetEnv.Cmd" /ia64
rem bjam msvc architecture=ia64 stage --stagedir=stage_ia64
rem endlocal

endlocal