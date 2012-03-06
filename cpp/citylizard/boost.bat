setlocal

rem cd C:\Users\Sergey\Downloads\boost_1_45_0\
rem cd C:\Users\Sergey\Downloads\boost_1_46_0\
rem cd C:\Users\sshandar\Downloads\boost_1_46_1\
rem cd C:\Users\sshandar\Downloads\boost_1_47_0\
rem cd C:\Users\Sergey\Downloads\boost_1_47_0\
rem cd C:\Users\Sergey\Downloads\boost_1_48_0\
cd C:\Users\Sergey\Downloads\boost_1_49_0\

call bootstrap.bat

setlocal
bjam msvc architecture=x86 stage --stagedir=stage_x86
endlocal

setlocal
call "C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\SetEnv.Cmd" /x64
bjam msvc architecture=x86 address-model=64 stage --stagedir=stage_x86_64
endlocal

setlocal
call "C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\SetEnv.Cmd" /ia64
bjam msvc architecture=ia64 stage --stagedir=stage_ia64
endlocal

endlocal