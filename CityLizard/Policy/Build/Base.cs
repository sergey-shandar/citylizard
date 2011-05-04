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
        private static readonly S.Type[] TypeList =
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

        private static readonly int[] ConstList =
        {
            0,
            1,
        };

        private static readonly string[] MemberConstList =
        {
            "MinValue",
            "MaxValue",
        };

        private static readonly CD.CodeBinaryOperatorType[] BinaryOperatorList =
        {
            CD.CodeBinaryOperatorType.Add,
            CD.CodeBinaryOperatorType.Subtract,
            CD.CodeBinaryOperatorType.Multiply,
            CD.CodeBinaryOperatorType.Divide,
        };

        private void Do(string root)
        {
            var t = Type("Base", IsPartial: true, IsStruct: true);
            foreach (var i in TypeList)
            {
                var s = i.ToString();
                var interface_ = "CityLizard.Policy.INumeric<" + s + ">";
                t.Add(TypeRef(interface_));
                var r = TypeRef(s);
                foreach (var c in ConstList)
                {
                    t.Add(
                        Property(interface_ + "._" + c.ToString(), r)
                            [Get()[Return(Primitive(c))]]);
                }
                foreach (var c in MemberConstList)
                {
                    t.Add(
                        Property(interface_ + "." + c, r)
                            [Get()[Return(Field(r, c))]]);
                }
                var a = Parameter(r, "a");
                var b = Parameter(r, "b");
                var ar = a.Ref();
                var br = b.Ref();
                foreach (var o in BinaryOperatorList)
                {
                    t.Add(                            
                        Method(interface_ + "." + o.ToString(), Return: r)
                            [a]
                            [b]
                            [Return(Cast(r, BinaryOperator(ar, o, br)))]);
                }
            }
            var u = Unit()[Namespace("CityLizard.Policy")[t]];
            var p = new CS.CSharpCodeProvider();
            using (var w =
                new IO.StreamWriter(IO.Path.Combine(root, "CityLizard/Policy/Base.cs")))
            {
                p.GenerateCodeFromCompileUnit(
                    u, w, new CD.Compiler.CodeGeneratorOptions());
            }
        }

        public static void Run(string root)
        {
            new Base().Do(root);
        }
    }
}
