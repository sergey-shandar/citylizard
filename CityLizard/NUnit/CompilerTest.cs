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
        [N.Test]
        public static void Load()
        {
            var u = new S.Compiler().Load(
                "../../../../www.w3.org/MarkUp/SCHEMA/xhtml11.xsd");
            //
            var t = new IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            //
            using (var w =
                new IO.StreamWriter(
                    "../../../XHtml/xhtml11.xsd.cs"))
            {
                w.Write(code);
            }
        }

        [N.Test]
        public static void LoadGraphMl()
        {
            var u = new S.Compiler().Load(
                "../../../../graphml.graphdrawing.org/xmlns/1.1/graphml.xsd");
            //
            var t = new IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            //
            using (var w =
                new IO.StreamWriter(
                    "../../../XHtml/graphml.xsd.cs"))
            {
                w.Write(code);
            }
        }
    }
}
