namespace CityLizard.Xml.Schema.Console
{
    using IO = System.IO;
    using CS = Microsoft.CSharp;
    using D = System.CodeDom;
    using S = System;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var u = new Compiler().Load(args[0]);
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
            catch (S.Exception e)
            {
                S.Console.WriteLine("error: " + e.ToString());
            }
        }
    }
}
