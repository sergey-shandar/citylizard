namespace CityLizard.PInvoke
{
	using R = System.Reflection;
    using I = System.Runtime.InteropServices;
	using S = System;
	using C = System.Collections.Generic;
    using T = System.Text;

    using System.Linq;
	
	public class CppBuilder
	{
		private static readonly C.Dictionary<S.Type, string> cppTypeMap = new C.Dictionary<S.Type, string>()
		{
			{ typeof(void), "void" },
			
			{ typeof(bool), "bool" },
			
			{ typeof(char), "wchar_t" },
			
			{ typeof(float), "float" },
			{ typeof(double), "double" },
			
			{ typeof(byte), "uint8_t" },
			{ typeof(sbyte), "int8_t" },
			{ typeof(ushort), "uint16_t" },
			{ typeof(short), "int16_t" },
			{ typeof(uint), "uint32_t" },
			{ typeof(int), "int32_t" },
			{ typeof(ulong), "uint64_t" },
			{ typeof(long), "int64_t" },

            { typeof(string), "BSTR *" },
		};

        private const string tab = "    ";
		
		private static string GetCppType(S.Type type)
		{
            string value;
            if (cppTypeMap.TryGetValue(type, out value))
            {
                return value;
            }
            return "::" + type.ToString().Replace(".", "::");
		}

        class Parameter
        {
            public readonly S.Type Type;
            public readonly bool IsIn;
            public readonly bool IsOut;

            public Parameter(S.Type type, bool isIn, bool isOut)
            {
                this.Type = type;
                this.IsIn = isIn;
                this.IsOut = isOut;
            }

            public Parameter(R.ParameterInfo parameter): 
                this(parameter.ParameterType, parameter.IsIn, parameter.IsOut)
            {
            }

            public string GetCpp()
            {
                return GetCppType(this.Type) + (this.IsOut ? " *" : "");
            }
        }

        private static C.IEnumerable<Parameter> GetCppParameters(R.MethodInfo method)
        {
            foreach (var p in method.GetParameters())
            {
                yield return new Parameter(p);
            }
            var type = method.ReturnType;
            if (type != typeof(void))
            {
                yield return new Parameter(type, false, true);
            }
        }

        private string GetCppMethod(R.MethodInfo method)
        {
            return
                GetCppType(method.ReturnType) +
                " " +
                method.Name +
                "(" +
                string.Join(
                    ", ", GetCppParameters(method).Select(p => p.GetCpp())) +
                ");\n";
        }
		
		public T.StringBuilder Build(R.Assembly assembly)
		{
            var result = new T.StringBuilder();

            // enumerations, structures and interfaces.
            foreach (var type in assembly.GetTypes())
            {
                var namespaces = type.Namespace.Split('.');
                result.AppendConcat(namespaces.Select(name => "namespace " + name + "\n" + "{\n"));
                if (type.IsEnum)
                {
                    result.Append("enum " + type.Name + "\n");
                    result.Append("{\n");
                    result.AppendConcat(type.GetEnumItems().Select(e => tab + e.Name + " = " + e.Value + ",\n"));
                    result.Append("};\n");
                }
                else if (type.IsValueType)
                {
                    result.Append("struct " + type.Name + "\n");
                    result.Append("{\n");
                    result.AppendConcat(type.GetFields().Select(f => tab + GetCppType(f.FieldType) + " " + f.Name + ";\n"));
                    result.Append("};\n");
                }
                else if (type.IsInterface)
                {
                    result.Append("class " + type.Name + "\n");
                    result.Append("{\n");
                    result.AppendConcat(type.GetMethods().Select(m => tab + GetCppMethod(m)));
                    result.Append("};\n");
                }
                result.AppendConcat(namespaces.Select(name => "}\n"));
            }

            // extern methods.
            result.AppendConcat(
                assembly.
                    GetTypes().
                    SelectMany(t => t.GetMethods()).
                    Where(method => (method.Attributes & R.MethodAttributes.PinvokeImpl) != 0).
                    Select(method => GetCppMethod(method)));

            return result;
		}
	}
}
