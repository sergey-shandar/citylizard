namespace CityLizard.Build
{
    using D = System.Diagnostics;

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

        static void Main(string[] args)
        {
            var r = Hg("manifest");
        }
    }
}
