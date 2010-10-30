namespace CityLizard.Build
{
    using S = System;
    using IO = System.IO;
    using CD = System.CodeDom;
    using R = System.Reflection;
    using CS = Microsoft.CSharp;
    using E = Microsoft.Build.BuildEngine;

    using System.Linq;

    class Program
    {
        static void Add<T>(CD.CodeCompileUnit u, string v)
        {
            var d = new CD.CodeAttributeDeclaration(
                new CD.CodeTypeReference(typeof(T)), 
                new CD.CodeAttributeArgument(
                    new CD.CodePrimitiveExpression(v)));
            u.AssemblyCustomAttributes.Add(d);
        }

        static void Main(string[] args)
        {
            var s = Hg.Summary();
            var version = 
                (s.Branch == "default" ? "0.0.0": s.Branch) + 
                "." + 
                s.Parent.RevisionNumber;
            var r = Hg.Locate(true);
            foreach (var i in r.Where(x => IO.Path.GetExtension(x) == ".csproj"))
            {
                S.Console.WriteLine("Project: " + i);
                var d = IO.Path.GetDirectoryName(i);
                var f = IO.Path.GetFileNameWithoutExtension(i);
                var u = new CD.CodeCompileUnit();
                Add<R.AssemblyVersionAttribute>(u, version);
                Add<R.AssemblyFileVersionAttribute>(u, version);
                Add<R.AssemblyTitleAttribute>(u, f);
                Add<R.AssemblyCompanyAttribute>(u, "CityLizard");
                Add<R.AssemblyProductAttribute>(u, f);
                Add<R.AssemblyCopyrightAttribute>(
                    u, "Copyright © CityLizard 2010");
                var p = new CS.CSharpCodeProvider();
                var dir = IO.Path.Combine(d, "Properties");
                IO.Directory.CreateDirectory(dir);
                using(var w = new IO.StreamWriter(IO.Path.Combine(
                    dir, "_AssemblyInfo.cs")))
                {
                    p.GenerateCodeFromCompileUnit(
                        u, w, new CD.Compiler.CodeGeneratorOptions());
                }
            }
            var e = new E.Engine();
            foreach (var i in 
                r.Where(
                    x => 
                        IO.Path.GetExtension(x) == ".sln" && 
                        IO.Path.GetFileNameWithoutExtension(x) != 
                            "CityLizard.Build"))
            {
                if (!e.BuildProjectFile(i))
                {
                    throw new S.Exception("Build error. Project: " +i);
                }
            }
        }
    }
}
