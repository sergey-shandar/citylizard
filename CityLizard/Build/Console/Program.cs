using System.Linq;

namespace CityLizard.Build.Console
{
    using IO = System.IO;
    using R = System.Reflection;
    using S = System;

    class Program
    {
        const string Company = "CityLizard";

        static void Main(string[] args)
        {
            // Set current directory to ".../CityLizard/"
            var dir = IO.Path.GetDirectoryName(R.Assembly.GetExecutingAssembly().Location);
            while (IO.Path.GetFileName(dir) != "CityLizard")
            {
                dir = IO.Path.GetDirectoryName(dir);
            }
            IO.Directory.SetCurrentDirectory(dir);
            S.Console.WriteLine(IO.Directory.GetCurrentDirectory());
            //
            var root = Hg.Hg.Root();
            var summary = Hg.Hg.Summary();
            var version = Build.Version(summary);
            //
            foreach (
                var file in 
                Hg.Hg.Locate().Where(
                    file => IO.Path.GetExtension(file) == ".vcproj"))
            {
                Build.CreateAssemblyInfo(
                    summary, Company, IO.Path.Combine(root, file));
            }
            //
            Policy.Build.Base.Run();
            //
            Build.BuildSolution(
                Hg.Hg.Locate().First(
                    file => IO.Path.GetFileName(file) == "CityLizard.sln"));
        }
    }
}
