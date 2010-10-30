namespace CityLizard.CodeDom
{
    using C = System.Collections.Generic;

    using Extension;

    public static class CSharp
    {
        public static C.HashSet<string> KeywordSet = new C.HashSet<string>
        {
            "abstract", "event", "new", "struct",
            "as", "explicit", "null", "switch",
            "base", "extern", "object", "this",
            "bool", "false", "operator", "throw",
            "break", "finally", "out", "true",
            "byte", "fixed", "override", "try",
            "case", "float", "params", "typeof",
            "catch", "for", "private", "uint",
            "char", "foreach", "protected", "ulong",
            "checked", "goto", "public", "unchecked",
            "class", "if", "readonly", "unsafe",
            "const", "implicit", "ref", "ushort",
            "continue", "in", "return", "using",
            "decimal", "int", "sbyte", "virtual",
            "default", "interface", "sealed", "volatile",
            "delegate", "internal", "short", "void",
            "do", "is", "sizeof", "while",
            "double", "lock", "stackalloc",
            "else", "long", "static",
            "enum", "namespace", "string",
        };

        public static string Name(string n)
        {
            var newName = n.Replace('.', '_').Replace('-', '_');
            if (char.IsDigit(newName[0]))
            {
                newName = "_" + newName;
            }
            return (KeywordSet.Contains(newName) ? "@" : "") + newName;
        }

        public const string Http = "http://";

        public static string Namespace(string s)
        {
            if (s.StartsWith(Http))
            {
                s = s.Remove(0, Http.Length);
            }
            var names = s.Split('/');
            var result = "";
            foreach (var n in names)
            {
                if (n != string.Empty)
                {
                    var newName = Name(n);
                    if (result == string.Empty)
                    {
                        result = newName;
                    }
                    else
                    {
                        result += "." + newName;
                    }
                }
            }
            return result;
        }
    }
}
