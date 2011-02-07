namespace CityLizard.Policy.Build
{
    using C = CodeDom;
    using S = System;
    using CS = Microsoft.CSharp;
    using CD = System.CodeDom;
    using IO = System.IO;

    using System.Linq;

    public class Base: C.Code
    {
        private static S.Type[] TypeList =
        {
            typeof(byte),
            typeof(sbyte),

            typeof(ushort),
            typeof(short),

            typeof(uint),
            typeof(int),

            typeof(ulong),
            typeof(long),

            typeof(decimal),
            typeof(float),
            typeof(double),
        };

        private void Do()
        {
            var t = Type("Base", IsPartial: true, IsStruct: true);
            foreach (var i in TypeList)
            {
                var s = i.ToString();
                var interface_ = "CityLizard.Policy.INumeric<" + s + ">";
                t.Add(TypeRef(interface_));
                var r = TypeRef(s);
                t.Add(
                    Method(interface_ + ".Zero", Return: r)
                        [Return(Primitive(0))
                        ]);
                var a = Parameter(r, "a");
                var b = Parameter(r, "b");
                t.Add(
                    Method(interface_ + ".Add", Return: r)
                        [a]
                        [b]
                        [Return(
                            Cast(
                                r, 
                                BinaryOperator(
                                    a.Ref(), 
                                    CD.CodeBinaryOperatorType.Add, 
                                    b.Ref())))
                        ]);
            }
            var u = Unit()[Namespace("CityLizard.Policy")[t]];
            var p = new CS.CSharpCodeProvider();
            using (var w = new IO.StreamWriter("Policy/Base.cs"))
            {
                p.GenerateCodeFromCompileUnit(
                    u, w, new CD.Compiler.CodeGeneratorOptions());
            }
        }

        public static void Run()
        {
            new Base().Do();
        }
    }
}
