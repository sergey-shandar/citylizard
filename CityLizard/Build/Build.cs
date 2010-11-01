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

    public static class Build
    {
        static readonly G.Dictionary<string, string> GlobalProperty =
            new G.Dictionary<string, string>
            {
                { "Configuration", "Debug" },
                { "Platform", "Any CPU" },
            };

        /// <summary>
        /// Build the solution.
        /// </summary>
        /// <param name="path"></param>
        public static void BuildSolution(string path)
        {
            var pc = new E.ProjectCollection();
            var bp = new Ex.BuildParameters(pc);

            var BuidlRequest = new Ex.BuildRequestData(
                path,
                GlobalProperty,
                null,
                new string[] { "Build" },
                null);

            var buildResult = Ex.BuildManager.DefaultBuildManager.Build(
                bp, BuidlRequest);
        }

        /// <summary>
        /// Add declaration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="u"></param>
        /// <param name="v"></param>
        private static void Add<T>(CD.CodeCompileUnit u, string v)
        {
            u.AssemblyCustomAttributes.AddDeclarationString<T>(v);
        }

        /// <summary>
        /// Version.
        /// </summary>
        /// <returns></returns>
        public static string Version(Hg.Hg.SummaryType s)
        {
            return (s.Branch == "default" ? "0.0.0" : s.Branch) +
                "." +
                s.Parent.RevisionNumber;
        }

        /// <summary>
        /// Create AssemblyInfo.cs.
        /// </summary>
        /// <param name="company"></param>
        /// <param name="i"></param>
        public static void CreateAssemblyInfo(
            Hg.Hg.SummaryType s, string company, string i)
        {
            var d = IO.Path.GetDirectoryName(i);
            var f = IO.Path.GetFileNameWithoutExtension(i);
            var u = new CD.CodeCompileUnit();
            var version = Version(s);
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
}
