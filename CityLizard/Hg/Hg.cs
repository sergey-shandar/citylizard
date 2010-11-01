namespace CityLizard.Hg
{
    using D = System.Diagnostics;
    using IO = System.IO;
    using CD = System.CodeDom;
    using R = System.Reflection;
    using CS = Microsoft.CSharp;

    using CityLizard.CodeDom.Extension;

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

            public string Version()
            {
                return (this.Branch == "default" ? "0.0.0" : this.Branch) +
                    "." +
                    this.Parent.RevisionNumber;
            }

            private static void Add<T>(CD.CodeCompileUnit u, string v)
            {
                u.AssemblyCustomAttributes.AddDeclarationString<T>(v);
            }

            public void CreateAssemblyInfo(string company, string i)
            {
                var d = IO.Path.GetDirectoryName(i);
                var f = IO.Path.GetFileNameWithoutExtension(i);
                var u = new CD.CodeCompileUnit();
                var version = this.Version();
                Add<R.AssemblyVersionAttribute>(u, version);
                Add<R.AssemblyVersionAttribute>(u, version);
                Add<R.AssemblyFileVersionAttribute>(u, version);
                Add<R.AssemblyTitleAttribute>(u, f);
                Add<R.AssemblyCompanyAttribute>(u, company);
                Add<R.AssemblyProductAttribute>(u, f);
                Add<R.AssemblyCopyrightAttribute>(
                    u, "Copyright © " + company + " 2010");
                var p = new CS.CSharpCodeProvider();
                var dir = IO.Path.Combine(d, "Properties");
                IO.Directory.CreateDirectory(dir);
                var a = IO.Path.Combine(dir, "AssemblyInfo.cs");
                using (var w = new IO.StreamWriter(a))
                {
                    p.GenerateCodeFromCompileUnit(
                        u, w, new CD.Compiler.CodeGeneratorOptions());
                }
            }
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
