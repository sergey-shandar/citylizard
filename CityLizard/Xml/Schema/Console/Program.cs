namespace CityLizard.XmlSchema.Console
{
    using IO = System.IO;
    using CS = Microsoft.CSharp;
    using D = System.CodeDom;

    class Program
    {
        static void Main(string[] args)
        {
            var u = Schema.Load(args[0]);
            //
            var t = new IO.StringWriter();
            new CS.CSharpCodeProvider().GenerateCodeFromCompileUnit(
                u, t, new D.Compiler.CodeGeneratorOptions());
            var code = t.ToString();
            //
            using (var w = new IO.StreamWriter(args[1]))
            {
                w.Write(code);
            }
        }
    }
}
