namespace CityLizard.Build.Console
{
    using IO = System.IO;
    using R = System.Reflection;
    using S = System;

    class Program
    {
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
        }
    }
}
