namespace CityLizard.Hg
{
    using D = System.Diagnostics;

    public static class Hg
    {
        public static string[] Command(string arguments)
        {
            D.ProcessStartInfo p = new D.ProcessStartInfo();
            p.FileName = "hg";
            p.Arguments = arguments;
            p.UseShellExecute = false;
            p.RedirectStandardOutput = true;
            D.Process x = D.Process.Start(p);
            return x.StandardOutput.ReadToEnd().Split('\n');
        }

        public static string[] Manifest()
        {
            return Command("manifest");
        }

        /// <summary>
        /// Print files under Mercurial control in the working directory whose 
        /// names match the given patterns.
        /// </summary>
        /// <param name="fullpath">
        /// print complete paths from the filesystem root
        /// </param>
        /// <returns></returns>
        public static string[] Locate(bool fullpath/* = false*/)
        {
            return Command("locate" + (fullpath ? " -f": ""));
        }

        public class ChangeSet
        {
            public int RevisionNumber;
            public string Id;
            public string Tags;
            public ChangeSet(string v)
            {
                int i = v.IndexOf(":");
                this.RevisionNumber = int.Parse(v.Substring(0, i));
                int s = v.IndexOf(" ");
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
            string[] r = Command("summary");
            SummaryType x = new SummaryType();
            x.Parent = new ChangeSet(Get(r[0]));
            x.Message = r[1];
            x.Branch = Get(r[2]);
            x.Commit = Get(r[3]);
            x.Update = Get(r[4]);
            return x;
        }

        public static string Root()
        {
            return Command("root")[0];
        }
    }
}
