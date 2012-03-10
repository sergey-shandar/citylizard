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
		private static readonly C.Dictionary<S.Type, string> cppTypeMap = 
            new C.Dictionary<S.Type, string>()
		    {
			    { typeof(void), "void" },
			
                // BOOL vs. VARIANT_BOOL vs. BOOLEAN vs. bool
                // http://blogs.msdn.com/b/oldnewthing/archive/2004/12/22/329884.aspx 
                //
                // type: BOOL 
                // size: 4
                // typedef: int
                //
                // type: BOOLEAN
                // size: 1
                // typedef BYTE
                //
                // type: VARIANT_BOOL
                // size: 2
                // typedef short
                //
                // type: bool
                // size: 1
                // typedef
                //
                // .NET marshal C array of VARIANT_BOOL as a C array of BOOL
                // http://connect.microsoft.com/VisualStudio/feedback/details/470491/net-marshal-c-array-of-variant-bool-as-a-c-array-of-bool
                //
                // tlbimp converts the VARIANT_BOOL structure field to .NET ushort structure field
                // https://connect.microsoft.com/VisualStudio/feedback/details/344203/tlbimp-converts-the-variant-bool-structure-field-to-net-ushort-structure-field
                //
                // Incorrect marshalling of VARIANT_BOOL
                // http://clrinterop.codeplex.com/workitem/3009?ProjectName=clrinterop
                //
                // TLBIMP converting C array of VARIANT_BOOL as C array of BOOL.
                // http://clrinterop.codeplex.com/workitem/3803
			    { typeof(bool), "::BOOL" },
			
			    { typeof(char), "wchar_t" },
			
			    { typeof(float), "float" },
			    { typeof(double), "double" },
			
			    { typeof(byte), "::uint8_t" },
			    { typeof(sbyte), "::int8_t" },
			    { typeof(ushort), "::uint16_t" },
			    { typeof(short), "::int16_t" },
			    { typeof(uint), "::uint32_t" },
			    { typeof(int), "::int32_t" },
			    { typeof(ulong), "::uint64_t" },
			    { typeof(long), "::int64_t" },

                { typeof(string), "::BSTR *" },
		    };

        private static readonly C.Dictionary<I.CallingConvention, string> cppCallingConventionMap =
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
		
		private static string GetCppType(S.Type type)
		{
            string value;
            if (cppTypeMap.TryGetValue(type, out value))
            {
                return value;
            }
            if (type.IsArray)
            {
                return "::SAFEARRAY *";
            }
            value = "::" + type.ToString().Replace(".", "::");
            if (type.IsEnum)
            {
                value += "::value_type";
            }
            return value;
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

        private static bool IsPreserveSig(R.MethodInfo method)
        {
            return (method.GetMethodImplementationFlags() & R.MethodImplAttributes.PreserveSig) != 0;
        }

        private static C.IEnumerable<Parameter> GetCppParameters(R.MethodInfo method)
        {
            foreach (var p in method.GetParameters())
            {
                yield return new Parameter(p);
            }
            if (!IsPreserveSig(method))
            {
                var type = method.ReturnType;
                if (type != typeof(void))
                {
                    yield return new Parameter(type, false, true);
                }
            }
        }

        private static string GetCppReturnType(R.MethodInfo method)
        {
            return IsPreserveSig(method) ? GetCppType(method.ReturnType): "::HRESULT";
        }

        private I.CallingConvention GetCallingConvention(R.MethodInfo method)
        {
            var attributes = method.GetCustomAttributes(typeof(I.DllImportAttribute), true);
            return attributes.Length == 0 ?
                I.CallingConvention.StdCall :
                ((I.DllImportAttribute)attributes[0]).CallingConvention;
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
            result.AppendLine("#include <boost/cstdint.hpp>");
            result.AppendLine("#pragma warning(push)");
            // warning C4146: unary minus operator applied to unsigned type, result still unsigned
            result.AppendLine("#pragma warning(disable: 4146)");

            // enumerations, structures and interfaces.
            foreach (var type in assembly.GetTypes())
            {
                var namespaces = type.Namespace.Split('.');
                result.AppendLineConcat(namespaces.Select(name => "namespace " + name + eol + "{"));

                // enum
                if (type.IsEnum)
                {
                    var valueType = GetCppType(type.GetEnumValueType());
                    result.AppendLine("namespace " + type.Name);
                    result.AppendLine("{");
                    result.AppendLine(tab + "typedef " + valueType + " value_type;");
                    result.AppendLineConcat(type.GetEnumItems().Select(e => tab + valueType + " const " + e.Name + " = " + e.Value + ";"));
                    result.AppendLine("}");
                }
                // struct
                else if (type.IsValueType)
                {
                    result.AppendLine("struct " + type.Name);
                    result.AppendLine("{");
                    result.AppendLineConcat(type.GetFields().Select(f => tab + GetCppType(f.FieldType) + " " + f.Name + ";"));
                    result.AppendLine("};");
                }
                // interface
                else if (type.IsInterface)
                {
                    result.AppendLine("class " + type.Name);
                    result.AppendLine("{");
                    result.AppendLineConcat(type.GetMethods().Select(m => tab + GetCppMethod(m)));
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
