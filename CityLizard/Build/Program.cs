namespace CityLizard.Build
{
    using D = System.Diagnostics;
    using S = System;
    using IO = System.IO;

    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var s = Hg.Hg.Summary();
            var version = 
                (s.Branch == "default" ? "0.0.0": s.Branch) + 
                "." + 
                s.Parent.RevisionNumber;
            var r = Hg.Hg.Manifest();
            foreach (var i in r.Where(x => x.EndsWith(".csproj")))
            {
                S.Console.WriteLine("Project: " + i);
                var d = IO.Path.GetDirectoryName(i);
            }
        }
    }
}
