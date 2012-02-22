namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;
    using IO = System.IO;
    using CS = Microsoft.CSharp;
    using D = System.CodeDom;

    using S = CityLizard.Xml.Schema;

    [N.TestFixture]
    public static class CompilerTest
    {
        private const string thirdParty = "../../../../../third_party/";

        [N.Test]
        public static void Load()
        {
            var u = new S.Compiler().Load(IO.Path.Combine(
                thirdParty, "www.w3.org/MarkUp/SCHEMA/xhtml11.xsd"));
            //
            var t = new IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            //
            using (var w =
                new IO.StreamWriter(
                    "../../../TypedDom/www_w3_org._1999.xhtml/X.xsd.cs"))
            {
                w.Write(code);
            }
        }

        [N.Test]
        public static void LoadGraphML()
        {
            var u = new S.Compiler().Load(IO.Path.Combine(
                thirdParty, "graphml.graphdrawing.org/xmlns/1.1/graphml.xsd"));
            //
            var t = new IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            //
            using (var w =
                new IO.StreamWriter(
                    "../../../TypedDom/graphml_graphdrawing_org.xmlns/X.xsd.cs"))
            {
                w.Write(code);
            }
        }

        [N.Test]
        public static void LoadSvg()
        {
            var u = new S.Compiler().Load(IO.Path.Combine(
                thirdParty, "www.w3.org/TR/2002/WD-SVG11-20020108/SVG.xsd"));
            //
            var t = new IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            //
            using (var w =
                new IO.StreamWriter(
                    "../../../TypedDom/www_w3_org._2000.svg/X.xsd.cs"))
            {
                w.Write(code);
            }
        }

        /*
        [N.Test]
        public static void LoadNuGet()
        {
            var u = new S.Compiler().Load(IO.Path.Combine(
                thirdParty, "nuget.codeplex.com/nuspec.xsd"));
            //
            var t = new IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            //
            using (var w =
                new IO.StreamWriter(
                    "../../../TypedDom/schemas_microsoft_com.packaging._2010._07.nuspec_xsd/X.xsd.cs"))
            {
                w.Write(code);
            }
        }
         * */

        [N.Test]
        public static void LoadLikeNuGet()
        {
            var u = new S.Compiler().Load("../../nuspec.xsd");
            //
            var t = new IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            //
            using (var w = new IO.StreamWriter("nuspec_xsd.xsd.cs"))
            {
                w.Write(code);
            }
        }
    }
}
