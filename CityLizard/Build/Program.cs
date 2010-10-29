namespace CityLizard.Build
{
    using D = System.Diagnostics;
    using S = System;

    using System.Linq;

    class Program
    {
        static class Hg
        {
            public static string[] Command(string arguments)
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

            public static string[] Manifest()
            {
                return Command("manifest");
            }

            public class ChangeSet
            {
                public int RevisionNumber;
                public string Id;
                public string Tags;
                public ChangeSet(string v)
                {
                    var i = v.IndexOf(":");
                    this.RevisionNumber = int.Parse(v.Substring(0, i));
                    var s = v.IndexOf(" ");
                    ++i;
                    this.Id = v.Substring(i, s - i);
                    this.Tags = v.Substring(s + 1);
                }
            }

            public class SummaryType
            {
                public ChangeSet Parent;
                public string Message;
                public string Branch;
                public string Commit;
                public string Update;
            }

            private static string Get(string v)
            {
                return v.Substring(v.IndexOf(": ") + 2);
            }

            public static SummaryType Summary()
            {
                var r = Command("summary");
                return new SummaryType
                {
                    Parent = new ChangeSet(Get(r[0])),
                    Message = r[1],
                    Branch = Get(r[2]),
                    Commit = Get(r[3]),
                    Update = Get(r[4]),
                };
            }
        }

        static void Main(string[] args)
        {
            var s = Hg.Summary();
            var version = 
                (s.Branch == "default" ? "0.0.0": s.Branch) + 
                "." + 
                s.Parent.RevisionNumber;
            var r = Hg.Manifest();
            foreach (var i in r.Where(x => x.EndsWith(".csproj")))
            {
                S.Console.WriteLine(i);
            }
        }
    }
}
