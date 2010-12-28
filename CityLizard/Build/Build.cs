namespace CityLizard.Build
{
    using E = Microsoft.Build.Evaluation;
    using Ex = Microsoft.Build.Execution;
    using G = System.Collections.Generic;
    using IO = System.IO;
    using CD = System.CodeDom;
    using R = System.Reflection;
    using CS = Microsoft.CSharp;

    using CityLizard.CodeDom.Extension;

    /// <summary>
    /// Build utilities.
    /// </summary>
    public static class Build
    {
        /// <summary>
        /// Build configuration.
        /// </summary>
        private static readonly G.Dictionary<string, string> GlobalProperty =
            new G.Dictionary<string, string>
            {
                { "Configuration", "Debug" },
                { "Platform", "Any CPU" },
            };

        /// <summary>
        /// Build the solution.
        /// </summary>
        /// <param name="path">A path to the solution.</param>
        public static Ex.BuildResult BuildSolution(string path)
        {
            var pc = new E.ProjectCollection();
            var bp = new Ex.BuildParameters(pc);

            var BuidlRequest = new Ex.BuildRequestData(
                path,
                GlobalProperty,
                null,
                new string[] { "Build" },
                null);

            return Ex.BuildManager.DefaultBuildManager.Build(
                bp, BuidlRequest);
        }

        /// <summary>
        /// Add attribute declaration.
        /// </summary>
        /// <typeparam name="T">Attribute.</typeparam>
        /// <param name="u">Compilation unit.</param>
        /// <param name="v">Value.</param>
        private static void Add<T>(CD.CodeCompileUnit u, string v)
        {
            u.AssemblyCustomAttributes.AddDeclarationString<T>(v);
        }
       
        /// <summary>
        /// Version. Format: Branch.RevisionNumber.
        /// </summary>
        /// <param name="s">Mercurial summary.</param>
        /// <returns>Version.</returns>
        public static string Version(Hg.Hg.SummaryType s)
        {
            return (s.Branch == "default" ? "0.0.0" : s.Branch) +
                "." +
                s.Parent.RevisionNumber;
        }

        const string Silverlight = ".Silverlight";

        /// <summary>
        /// Creates AssemblyInfo.cs.
        /// </summary>
        /// <param name="s">Summary.</param>
        /// <param name="company">Company name.</param>
        /// <param name="solution">Path to the solution.</param>
        public static void CreateAssemblyInfo(
            Hg.Hg.SummaryType s, string company, string solution)
        {
            var d = IO.Path.GetDirectoryName(solution);
            var f = IO.Path.GetFileNameWithoutExtension(solution);
            string platform;
            if (f.EndsWith(Silverlight))
            {
                f = f.Substring(0, f.Length - Silverlight.Length);
                platform = "Silverlight";
            }
            else
            {
                platform = ".NET Framework";
            }
            var u = new CD.CodeCompileUnit();
            var version = Version(s);
            Add<R.AssemblyVersionAttribute>(u, version);
            Add<R.AssemblyFileVersionAttribute>(u, version);
            Add<R.AssemblyTitleAttribute>(u, f + " for " + platform);
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
}
