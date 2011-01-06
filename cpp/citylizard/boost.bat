cd C:\Users\Sergey\Downloads\boost_1_45_0\
bjam msvc architecture=x86 stage --stagedir=stage_x86
bjam msvc architecture=x86 address-model=64 stage --stagedir=stage_x86_64
bjam msvc architecture=ia64 stage --stagedir=stage_ia64
