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
                return this.Info.ToCppType() + (this.IsOut ? " *" : "");
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
                method.ReturnParameter.ToCppType() : 
                CppType.HResult;
        }

        private I.CallingConvention GetCallingConvention(R.MethodInfo method)
        {
            var attribute = 
                method.GetCustomAttribute<I.DllImportAttribute>(true);
            return attribute != null ? 
                attribute.CallingConvention : 
                I.CallingConvention.StdCall;
        }

        private string GetCppMethod(R.MethodInfo method)
        {
            return
                GetCppReturnType(method) +
                " " + 
                cppCallingConventionMap[GetCallingConvention(method)] + 
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
                    var valueType = type.GetEnumValueType().ToCppType(type);
                    result.AppendLine("namespace " + type.Name);
                    result.AppendLine("{");
                    result.AppendLine(tab + "typedef " + valueType + " value_type;");
                    result.AppendLineConcat(
                        type.
                            GetEnumItems().
                            Select(
                                e => 
                                    tab + 
                                    valueType + 
                                    " const " + 
                                    e.Name + 
                                    " = " + 
                                    e.Value + 
                                    ";"));
                    result.AppendLine("}");
                }
                // struct
                else if (type.IsValueType)
                {
                    result.AppendLine("struct " + type.Name);
                    result.AppendLine("{");
                    result.AppendLineConcat(
                        type.
                            GetFields().
                            Select(
                                f => tab + f.ToCppType() + " " + f.Name + ";"));
                    result.AppendLine("};");
                }
                // interface
                else if (type.IsInterface)
                {
                    result.AppendLine("class " + type.Name);
                    result.AppendLine("{");
                    result.AppendLineConcat(
                        type.GetMethods().Select(m => tab + GetCppMethod(m)));
                    result.AppendLine("};");
                }
                result.AppendLineConcat(namespaces.Select(name => "}"));
            }

            // extern methods.
            result.AppendLineConcat(
                assembly.
                    GetTypes().
                    SelectMany(t => t.GetMethods()).
                    Where(method => (method.Attributes & R.MethodAttributes.PinvokeImpl) != 0).
                    Select(method => "extern \"C\" __declspec(dllexport) " + GetCppMethod(method)));

            result.AppendLine("#pragma warning(pop)");

            return result;
		}
	}
}
