﻿namespace CityLizard.Build
{
    using S = System;
    using IO = System.IO;
    using CD = System.CodeDom;
    using R = System.Reflection;
    using CS = Microsoft.CSharp;
    using E = Microsoft.Build.Evaluation;
    using Ex = Microsoft.Build.Execution;
    using G = System.Collections.Generic;

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

        static readonly string[] ConfigurationList =
        {
            "Debug", 
            "Release",
        };

        static readonly G.Dictionary<string, string> GlobalProperty = 
            new G.Dictionary<string, string>
            {
                { "Configuration", "Debug" },
                { "Platform", "Any CPU" },
            };

        static void BuildSolution(string path)
        {
            S.Console.WriteLine("solution: " + path);

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
                    dir, "AssemblyInfo.cs")))
                {
                    p.GenerateCodeFromCompileUnit(
                        u, w, new CD.Compiler.CodeGeneratorOptions());
                }
            }
            
            // build CityLizard.sln.
            var sln = r.First(x =>  IO.Path.GetFileName(x) == "CityLizard.sln");
            BuildSolution(sln);

            // xhtml11.xsd.cs
            S.Console.WriteLine("XHTML");
            var console = r.First(x => 
                IO.Path.GetFileName(x) == 
                    "CityLizard.Xml.Schema.Console.csproj");
            console = IO.Path.Combine(
                IO.Path.GetDirectoryName(console),
                "bin",
                "Debug",
                "CityLizard.Xml.Schema.Console.exe");
            var source = r.First(x => IO.Path.GetFileName(x) == "xhtml11.xsd");
            var xhtmlSln = r.First(x => 
                IO.Path.GetFileName(x) == "CityLizard.XHtml.sln");
            var xhtmlDir = IO.Path.GetDirectoryName(xhtmlSln);
            Process.Start(
                console, 
                '"' + 
                source + 
                "\" \"" +
                IO.Path.Combine(xhtmlDir, "xhtml11.xsd.cs") +
                '"');

            // build CityLizard.XHtml.sln.
            BuildSolution(xhtmlSln);

            var root = Hg.Root();
            //
            var zip = IO.Path.Combine(
                root, "CityLizard.XHtml." + version + ".zip"); 
            var l = Process.Start(
                "C:\\Program Files\\7-Zip\\7z.exe",
                "a \"" +
                zip +
                "\" \"" +
                IO.Path.Combine(xhtmlDir, "bin", "Debug", "*.dll") +
                "\" \"" +
                IO.Path.Combine(root, "CityLizard", "license.txt") +
                "\"");
        }
    }
}
