namespace CityLizard.Build
{
    using D = System.Diagnostics;
    using S = System;

    using System.Linq;

    class Program
    {
        static string[] Hg(string arguments)
        {
            var p = new D.ProcessStartInfo
            {
                FileName = "hg",
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            var x = D.Process.Start(p);
            return x.StandardOutput.ReadToEnd().Split('\n');
        }

        const string Version = "1.1";

        static void Main(string[] args)
        {
            var r = Hg("manifest");
            foreach (var i in r.Where(x => x.EndsWith(".csproj")))
            {
                S.Console.WriteLine(i);
            }
        }
    }
}
