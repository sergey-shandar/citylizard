setlocal
set PATH=%PATH%;C:\programs
set MAJOR_VERSION=1.54
set VERSION=%MAJOR_VERSION%.0.1

mkdir build
cd build

nuget.exe pack ..\boost.nuspec -Version %VERSION%
nuget.exe push boost.%VERSION%.nupkg

rem BOOST_ATOMIC_DYN_LINK
call ..\lib.bat atomic
rem BOOST_CHRONO_DYN_LINK
call ..\lib.bat chrono
rem BOOST_CONTEXT_DYN_LINK
call ..\lib.bat context

rem BOOST_COROUTINE_DYN_LINK but no DLLs created.
call ..\lib.bat coroutine

rem BOOST_DATE_TIME_DYN_LINK
call ..\lib.bat date_time

rem no auto_link
call ..\lib.bat exception

rem BOOST_FILESYSTEM_DYN_LINK
call ..\lib.bat filesystem
rem BOOST_GRAPH_DYN_LINK
call ..\lib.bat graph
rem BOOST_IOSTREAMS_DYN_LINK
call ..\lib.bat iostreams
rem BOOST_LOCALE_DYN_LINK
call ..\lib.bat locale
rem BOOST_LOG_DYN_LINK
call ..\lib.bat log
rem BOOST_LOG_SETUP_DYN_LINK
call ..\lib.bat log_setup

rem BOOST_MATH_TR1_DYN_LINK
call ..\lib.bat math_c99
call ..\lib.bat math_c99f
call ..\lib.bat math_c99l
call ..\lib.bat math_tr1
call ..\lib.bat math_tr1f
call ..\lib.bat math_tr1l

rem BOOST_TEST_DYN_LINK
call ..\lib.bat prg_exec_monitor
call ..\lib.bat unit_test_framework
rem static only
call ..\lib.bat test_exec_monitor

rem BOOST_PROGRAM_OPTIONS_DYN_LINK
call ..\lib.bat program_options
rem BOOST_RANDOM_DYN_LINK
call ..\lib.bat random
rem BOOST_REGEX_DYN_LINK
call ..\lib.bat regex

rem BOOST_SERIALIZATION_DYN_LINK, serialization/
call ..\lib.bat serialization
rem BOOST_SERIALIZATION_DYN_LINK, archive/
call ..\lib.bat wserialization

rem BOOST_SIGNALS_DYN_LINK
call ..\lib.bat signals
rem BOOST_SYSTEM_DYN_LINK
call ..\lib.bat system
rem BOOST_THREAD_DYN_LINK
call ..\lib.bat thread
rem BOOST_TIMER_DYN_LINK
call ..\lib.bat timer
rem BOOST_WAVE_DYN_LINK
call ..\lib.bat wave

cd ..

endlocal
