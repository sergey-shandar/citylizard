setlocal

set PATH=%PATH%;C:\programs

nuget.exe pack boost_atomic_src.nuspec
nuget.exe pack boost_chrono_src.nuspec
nuget.exe pack boost_system_src.nuspec
nuget.exe pack boost_coroutine_src.nuspec
nuget.exe pack boost_date_time_src.nuspec
nuget.exe pack boost_exception_src.nuspec

nuget.exe pack boost_python.nuspec
nuget.exe pack boost_mpi.nuspec

endlocal
