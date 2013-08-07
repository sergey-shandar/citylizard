setlocal
set PATH=%PATH%C:\Windows\Microsoft.NET\Framework\v4.0.30319;
msbuild desktop.sln
msbuild web.sln
msbuild windows8.sln
msbuild wp.sln
msbuild psm.sln
endlocal