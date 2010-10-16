namespace CityLizard.NUnit
{
    using CS = Microsoft.CSharp;
    using D = System.CodeDom;
    using N = global::NUnit.Framework;
    using IO = System.IO;

    [N.TestFixture]
    public static class SchemaTest
    {
        [N.Test]
        public static void Load()
        {
            var u = Schema.Load(
                "../../../../www.w3.org/MarkUp/SCHEMA/xhtml11.xsd");
            //
            var t = new IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            //
            using (var w = 
                new IO.StreamWriter(
                    "../../../../../www.w3.org/1999/xhtml/html.xsd.cs"))
            {
                w.Write(code);
            }
        }
    }
}
