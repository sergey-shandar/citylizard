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
        private static readonly C.Dictionary<I.CallingConvention, string> 
            cppCallingConventionMap = 
            new C.Dictionary<I.CallingConvention, string>()
            {
                // Winapi is not actually a calling convention, but instead uses
                // the default platform calling convention. For example, on 
                // Windows the default is StdCall and on Windows CE.NET it is 
                // Cdecl.
                { I.CallingConvention.Winapi, "WINAPI" },
                { I.CallingConvention.Cdecl, "__cdecl" },
                { I.CallingConvention.StdCall, "__stdcall" },
            };

        private const string tab = "    ";

        private const string eol = "\r\n";

        class Parameter
        {
            public readonly R.ParameterInfo Info;
            public readonly bool IsIn;
            public readonly bool IsOut;

            public Parameter(R.ParameterInfo info, bool isIn, bool isOut)
            {
                this.Info = info;
                this.IsIn = isIn;
                this.IsOut = isOut;
            }

            public Parameter(R.ParameterInfo info): 
                this(info, info.IsIn, info.IsOut)
            {
            }

            public string GetCpp()
            {
                var cppType = this.Info.ToCppType();
                return cppType.Prefix + (this.IsOut ? "(*)" : "") + cppType.Suffix;
            }
        }

        private static C.IEnumerable<Parameter> GetCppParameters(
            R.MethodInfo method)
        {
            foreach (var p in method.GetParameters())
            {
                yield return new Parameter(p);
            }
            if (!method.IsPreserveSig())
            {
                var type = method.ReturnType;
                if (type != typeof(void))
                {
                    yield return new Parameter(
                        method.ReturnParameter, false, true);
                }
            }
        }

        private static string GetCppReturnType(R.MethodInfo method)
        {
            return method.IsPreserveSig() ? 
                method.ReturnParameter.ToCppType().Prefix :
                CppType.HResult;
        }

        private string GetCppMethod(R.MethodInfo method)
        {
            return
                GetCppReturnType(method) +
                " " + 
                cppCallingConventionMap[method.GetCallingConvention()] + 
                " " +
                method.Name +
                "(" +
                string.Join(
                    ", ", GetCppParameters(method).Select(p => p.GetCpp())) +
                ");";
        }

		public T.StringBuilder Build(R.Assembly assembly)
		{
            var result = new T.StringBuilder();

            result.AppendLine("#pragma once");
            result.AppendLine("#include <Windows.h>");
            result.AppendLine("#pragma warning(push)");
            // warning C4146: unary minus operator applied to unsigned type, 
            // result still unsigned
            result.AppendLine("#pragma warning(disable: 4146)");

            // enumerations, structures and interfaces.
            foreach (var type in assembly.GetTypes())
            {
                var namespaces = type.Namespace.Split('.');
                result.AppendLineConcat(
                    namespaces.Select(name => "namespace " + name + eol + "{"));

                // enum
                if (type.IsEnum)
                {
                    var valueType = type.GetEnumValueType().ToCppType(type, I.CharSet.Auto).Prefix;
                    result.AppendLine("namespace " + type.Name);
                    result.AppendLine("{");
                    result.AppendLine(tab + "typedef " + valueType + " value_type;");
                    foreach (var e in type.GetEnumItems())
                    {
                        result.AppendLine(
                            tab + 
                            valueType + 
                            " const " + 
                            e.Name + 
                            " = " + 
                            e.Value + 
                            ";");
                    }
                    result.AppendLine("}");
                }
                // struct
                else if (type.IsValueType)
                {
                    if (!type.IsLayoutSequential)
                    {
                        throw new S.Exception("not sequential layout");
                    }
                    var layout = type.StructLayoutAttribute;
                    result.AppendLine("#pragma pack(push, " + layout.Pack + ")");
                    result.AppendLine("struct " + type.Name);
                    result.AppendLine("{");
                    foreach(var f in 
                        type.GetFields(
                            R.BindingFlags.NonPublic | 
                            R.BindingFlags.Public | 
                            R.BindingFlags.Instance))
                    {
                        var cppType = f.ToCppType();
                        result.AppendLine(
                            tab + cppType.Prefix + " " + f.Name + cppType.Suffix + ";");
                    }
                    result.AppendLine("};");
                    result.AppendLine("#pragma pack(pop)");
                }
                // interface
                else if (type.IsInterface)
                {
                    result.AppendLine("class " + type.Name);
                    result.AppendLine("{");
                    foreach (var m in type.GetMethods())
                    {
                        result.AppendLine(tab + GetCppMethod(m));
                    }
                    result.AppendLine("};");
                }
                result.AppendLineConcat(namespaces.Select(name => "}"));
            }

            // extern methods.
            foreach (var method in 
                assembly.GetTypes().SelectMany(t => t.GetMethods()))
            {
                if ((method.Attributes & R.MethodAttributes.PinvokeImpl) != 0)
                {
                    result.AppendLine(
                        "extern \"C\" __declspec(dllexport) " + 
                        GetCppMethod(method));
                }
            }

            result.AppendLine("#pragma warning(pop)");

            return result;
		}
	}
}
