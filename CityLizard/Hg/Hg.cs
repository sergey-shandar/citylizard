namespace CityLizard.Hg
{
    using D = System.Diagnostics;

    /// <summary>
    /// Mercurial access.
    /// <see cref="http://en.wikipedia.org/wiki/Mercurial"/>
    /// </summary>
    public static class Hg
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

        /// <summary>
        /// The current revision of the project manifest.
        /// </summary>
        /// <returns>
        /// A list of version controlled files for the first parent of the 
        /// working directory.
        /// </returns>
        public static string[] Manifest()
        {
            return Command("manifest");
        }

        /// <summary>
        /// Locate files.
        /// </summary>
        /// <param name="fullpath">
        /// Return complete paths from the filesystem root.
        /// </param>
        /// <returns>
        /// Files under Mercurial control in the working directory.
        /// </returns>
        public static string[] Locate(bool fullpath = false)
        {
            return Command("locate" + (fullpath ? " -f": ""));
        }

        /// <summary>
        /// Change set.
        /// </summary>
        public class ChangeSet
        {
            /// <summary>
            /// Revision number.
            /// </summary>
            public int RevisionNumber;

            /// <summary>
            /// Id.
            /// </summary>
            public string Id;

            /// <summary>
            /// Tags.
            /// </summary>
            public string Tags;

            /// <summary>
            /// Constructor from string.
            /// </summary>
            /// <param name="v"></param>
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

        /// <summary>
        /// Summary.
        /// </summary>
        public class SummaryType
        {
            /// <summary>
            /// Parent change set.
            /// </summary>
            public ChangeSet Parent;

            /// <summary>
            /// Message.
            /// </summary>
            public string Message;

            /// <summary>
            /// Branch.
            /// </summary>
            public string Branch;

            /// <summary>
            /// Commit.
            /// </summary>
            public string Commit;

            /// <summary>
            /// Update.
            /// </summary>
            public string Update;
        }

        /// <summary>
        /// Get a field name.
        /// </summary>
        /// <param name="v">Field.</param>
        /// <returns>Name of field.</returns>
        private static string Get(string v)
        {
            return v.Substring(v.IndexOf(": ") + 2);
        }

        /// <summary>
        /// Summarize working directory state.
        /// </summary>
        /// <returns>
        /// A brief summary of the working directory state, including parents, 
        /// branch, commit status, and available updates.
        /// </returns>
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

        /// <summary>
        /// The root (top) of the current working directory.
        /// </summary>
        /// <returns>The root directory of the current repository.</returns>
        public static string Root()
        {
            return Command("root")[0];
        }
    }
}
