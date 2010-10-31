namespace CityLizard.Build
{
    using D = System.Diagnostics;

    static class Process
    {
        public static string[] Start(string name, string arguments)
        {
            var p = new D.ProcessStartInfo
            {
                FileName = name,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            var x = D.Process.Start(p);
            return x.StandardOutput.ReadToEnd().Split('\n');
        }
    }
}
