cd %1
echo using System.Reflection; > AssemblyInfo.cs
echo [assembly: AssemblyCompany("CityLizard")] >> AssemblyInfo.cs
echo [assembly: AssemblyCopyright("Copyright © CityLizard 2013")] >> AssemblyInfo.cs
echo [assembly: AssemblyVersion("%VERSION%")] >> AssemblyInfo.cs
echo [assembly: AssemblyFileVersion("%VERSION%")] >> AssemblyInfo.cs
echo [assembly: AssemblyTitle("%1")] >> AssemblyInfo.cs
echo [assembly: AssemblyProduct("%1")] >> AssemblyInfo.cs
cd ..