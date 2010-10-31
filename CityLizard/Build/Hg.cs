namespace CityLizard.Build
{
    internal static class Hg
    {
       public static string[] Command(string arguments)
        {
            return Process.Start("hg", arguments);
        }

        public static string[] Locate(bool fullpath = false)
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

        public static string Root()
        {
            return Command("root")[0];
        }
    }
}
