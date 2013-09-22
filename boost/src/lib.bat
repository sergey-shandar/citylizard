setlocal

set PATH=%PATH%;C:\programs

nuget.exe pack boost_atomic_src.nuspec
nuget.exe pack boost_chrono_src.nuspec
nuget.exe pack boost_system_src.nuspec
nuget.exe pack boost_coroutine_src.nuspec
nuget.exe pack boost_date_time_src.nuspec
nuget.exe pack boost_exception_src.nuspec
nuget.exe pack boost_filesystem_src.nuspec
nuget.exe pack boost_serialization_src.nuspec
nuget.exe pack boost_graph_src.nuspec
nuget.exe pack boost_regex_src.nuspec

rem nuget.exe pack boost_python.nuspec
rem nuget.exe push boost_python.1.54.0.6.nupkg

rem nuget.exe pack boost_mpi.nuspec
rem nuget.exe push boost_mpi.1.54.0.6.nupkg

rem nuget.exe pack boost_mpi_python.nuspec
rem nuget.exe push boost_mpi_python.1.54.0.3.nupkg

endlocal
