setlocal
set PATH=%PATH%;C:\programs

rem nuget.exe pack boost.nuspec

mkdir build
cd build

call ../lib.bat config
call ../lib.bat version
call ../lib.bat detail
call ../lib.bat preprocessor
call ../lib.bat type_traits
call ../lib.bat mpl detail preprocessor type_traits
call ../lib.bat utility
call ../lib.bat parameter utility
call ../lib.bat blank
call ../lib.bat ref
call ../lib.bat static_assert
call ../lib.bat fusion blank ref static_assert
call ../lib.bat concept_check
call ../lib.bat accumulators config version mpl parameter fusion concept_check
endlocal
