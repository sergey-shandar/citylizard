setlocal
set info=..\source\%1\AssemblyInfo.cs
echo using System.Reflection; > %info%
echo [assembly: AssemblyCompany("CityLizard")] >> %info%
echo [assembly: AssemblyCopyright("Copyright © CityLizard 2013")] >> %info%
echo [assembly: AssemblyVersion("%VERSION%")] >> %info%
echo [assembly: AssemblyFileVersion("%VERSION%")] >> %info%
echo [assembly: AssemblyTitle("%1")] >> %info%
echo [assembly: AssemblyProduct("%1")] >> %info%
endlocal
 