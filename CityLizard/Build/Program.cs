namespace CityLizard.Build
{
    using S = System;
    using IO = System.IO;
    using CD = System.CodeDom;
    using R = System.Reflection;
    using CS = Microsoft.CSharp;

    using System.Linq;
    using CodeDom.Extension;

    class Program
    {
        static void Main(string[] args)
        {
            var s = Hg.Hg.Summary();
            var version = 
                (s.Branch == "default" ? "0.0.0": s.Branch) + 
                "." + 
                s.Parent.RevisionNumber;
            var r = Hg.Hg.Locate(true);
            foreach (var i in r.Where(x => x.EndsWith(".csproj")))
            {
                S.Console.WriteLine("Project: " + i);
                var d = IO.Path.GetDirectoryName(i);
                var f = IO.Path.GetFileNameWithoutExtension(i);
                var u = new CD.CodeCompileUnit();
                u.AssemblyCustomAttributes.
                    AddDeclarationString<R.AssemblyVersionAttribute>(version);
                u.AssemblyCustomAttributes.
                    AddDeclarationString<R.AssemblyFileVersionAttribute>(
                        version);
                u.AssemblyCustomAttributes.
                    AddDeclarationString<R.AssemblyTitleAttribute>(f);
                u.AssemblyCustomAttributes.
                    AddDeclarationString<R.AssemblyCompanyAttribute>(
                        "CityLizard");
                u.AssemblyCustomAttributes.
                    AddDeclarationString<R.AssemblyProductAttribute>(f);
                u.AssemblyCustomAttributes.
                    AddDeclarationString<R.AssemblyCopyrightAttribute>(
                        "Copyright © CityLizard 2010");
                var p = new CS.CSharpCodeProvider();
                using(var w = new IO.StreamWriter(IO.Path.Combine(
                    d, "Properties\\AssemblyInfo.cs")))
                {
                    p.GenerateCodeFromCompileUnit(
                        u, w, new CD.Compiler.CodeGeneratorOptions());
                }
            }
        }
    }
}
