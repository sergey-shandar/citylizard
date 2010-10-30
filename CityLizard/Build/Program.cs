﻿namespace CityLizard.Build
{
    using S = System;
    using IO = System.IO;
    using CD = System.CodeDom;
    using R = System.Reflection;
    using CS = Microsoft.CSharp;

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
            foreach (var i in r.Where(x => x.EndsWith(".csproj")))
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
