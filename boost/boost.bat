cd C:\Users\Sergey\Downloads\boost_1_54_0\

call bootstrap.bat

setlocal
bjam msvc architecture=x86 stage --stagedir=stage_x86
endlocal

setlocal
bjam msvc architecture=x86 address-model=64 stage --stagedir=stage_x86_64
endlocal

setlocal
rem bjam msvc architecture=arm stage --stagedir=stage_arm
endlocal

endlocal