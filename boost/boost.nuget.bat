setlocal
set PATH=%PATH%;C:\programs
set VERSION=1.54.0.0

mkdir build
cd build

rem nuget.exe pack ..\boost.nuspec

call ..\lib.bat atomic
call ..\lib.bat chrono
call ..\lib.bat context
call ..\lib.bat coroutine
call ..\lib.bat date_time
call ..\lib.bat exception
call ..\lib.bat filesystem
call ..\lib.bat graph
call ..\lib.bat iostreams
call ..\lib.bat locale
call ..\lib.bat log
call ..\lib.bat log_setup
call ..\lib.bat math_c99
call ..\lib.bat math_c99f
call ..\lib.bat math_c99l
call ..\lib.bat math_tr1
call ..\lib.bat math_tr1f
call ..\lib.bat math_tr1l
call ..\lib.bat prg_exec_monitor
call ..\lib.bat program_options
call ..\lib.bat random
call ..\lib.bat regex
call ..\lib.bat serialization
call ..\lib.bat signals
call ..\lib.bat system
call ..\lib.bat test_exec_monitor
call ..\lib.bat thread
call ..\lib.bat timer
call ..\lib.bat unit_test_framework
call ..\lib.bat wave
call ..\lib.bat wserialization

cd ..

endlocal
