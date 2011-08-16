using System.Linq;

namespace CityLizard.Build.Console
{
    using IO = System.IO;
    using R = System.Reflection;
    using S = System;
    using D = System.Diagnostics;

    class Program
    {
        const string Company = "CityLizard";

        struct Pair
        {
            public string Input;
            public string OutputDir;
            public Pair(string input, string outputDir)
            {
                this.Input = input;
                this.OutputDir = outputDir;
            }
        }

        class XmlSchema
        {

            private readonly string Command;
            private readonly string Input;
            private readonly string Output;

            public XmlSchema(string root)
            {
                this.Command = IO.Path.Combine(
                    root,
                    "CityLizard\\Xml\\Schema\\Console\\bin\\Debug",
                    "CityLizard.Xml.Schema.Console.exe");
                this.Input = IO.Path.Combine(
                    root,
                    "third_party");
                this.Output = IO.Path.Combine(
                    root,
                    "CityLizard\\TypedDom");
            }

            public void Run(Pair pair)
            {
                var p = new D.ProcessStartInfo
                {
                    FileName = this.Command,
                    Arguments = 
                        IO.Path.Combine(this.Input, pair.Input) + 
                        " " + 
                        IO.Path.Combine(this.Output, pair.OutputDir, "X.xsd.cs"),
                    UseShellExecute = false,
                };
                D.Process.Start(p);
            }
        }

        static readonly Pair[] XmlSchemaList =
        {
            new Pair(
                "www.w3.org\\MarkUp\\SCHEMA\\xhtml5.xsd",
                "www_w3_org._1999.xhtml"),
            new Pair(
                "graphml.graphdrawing.org\\xmlns\\1.1\\graphml.xsd",
                "graphml_graphdrawing_org.xmlns"),
            new Pair(
                "www.w3.org\\TR\\2002\\WD-SVG11-20020108\\SVG.xsd",
                "www_w3_org._2000.svg"),
            new Pair(
                "nuget.codeplex.com\\nuspec.xsd",
                "schemas_microsoft_com.packaging._2010._07.nuspec_xsd"),
            new Pair(
                "wix.codeplex.com\\wix.xsd",
                "schemas_microsoft_com.wix.2006.wi"),
        };

        static void Main(string[] args)
        {
            //
            var root = Hg.Hg.Root();
            var summary = Hg.Hg.Summary();
            var version = Build.Version(summary);
            //
            foreach (
                var file in 
                Hg.Hg.Locate().Where(
                    file => IO.Path.GetExtension(file) == ".csproj"))
            {
                Build.CreateAssemblyInfo(
                    summary, Company, IO.Path.Combine(root, file));
            }
            //
            Policy.Build.Base.Run(root);
            //
            Build.BuildSolution(
                Hg.Hg.Locate().First(
                    file => IO.Path.GetFileName(file) == "CityLizard.sln"));
            //
            var xs = new XmlSchema(root);
            //
            foreach(var i in XmlSchemaList)
            {
                xs.Run(i);
            }
            //
            Build.BuildSolution(
                Hg.Hg.Locate().First(
                    file => IO.Path.GetFileName(file) == "CityLizard.TypedDom.sln"));
        }
    }
}
