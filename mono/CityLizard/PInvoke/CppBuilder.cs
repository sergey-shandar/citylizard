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

        /// <summary>
        /// See definition of VARIANT structure in OAdl.h
        /// </summary>
        private static C.Dictionary<I.UnmanagedType, string> cppUnmanagedTypeMap =
            new C.Dictionary<I.UnmanagedType, string>()
            {
                /// A 4-byte Boolean value (true != 0, false = 0). This is the 
                /// Win32 BOOL type.
                { I.UnmanagedType.Bool, "::BOOL" },

                /// A 1-byte signed integer. You can use this member to transform 
                /// a Boolean value into a 1-byte, C-style bool (true = 1, false = 0).
                { I.UnmanagedType.I1, "::CHAR" },

                /// A 1-byte unsigned integer.
                { I.UnmanagedType.U1, "::BYTE" },

                /// A 2-byte signed integer.
                { I.UnmanagedType.I2, "::SHORT" },

                /// A 2-byte unsigned integer.
                { I.UnmanagedType.U2, "::USHORT" },

                /// A 4-byte signed integer.
                { I.UnmanagedType.I4, "::LONG" },

                /// A 4-byte unsigned integer.
                { I.UnmanagedType.U4, "::ULONG" },

                /// An 8-byte signed integer.
                { I.UnmanagedType.I8, "::LONGLONG" },

                /// An 8-byte unsigned integer.
                { I.UnmanagedType.U8, "::ULONGLONG" },

                /// A 4-byte floating point number.
                { I.UnmanagedType.R4, "::FLOAT" },

                /// An 8-byte floating point number.
                { I.UnmanagedType.R8, "::DOUBLE" },

                /// Used on a System.Decimal to marshal the decimal value as a
                /// COM currency type instead of as a Decimal.
                { I.UnmanagedType.Currency, "::CY" },

                /// A Unicode character string that is a length-prefixed double 
                /// byte. You can use this member, which is the default string 
                /// in COM, on the String data type.
                { I.UnmanagedType.BStr, "::BSTR" },

                /// A single byte, null-terminated ANSI character string. You 
                /// can use this member on the System.String or 
                /// System.Text.StringBuilder data types
                { I.UnmanagedType.LPStr, "::LPSTR" },

                /// A 2-byte, null-terminated Unicode character string.
                /// Note that you cannot use the LPWStr value with an unmanaged 
                /// string unless the string was created using the unmanaged 
                /// CoTaskMemAlloc function.
                { I.UnmanagedType.LPWStr, "::LPWSTR" },

                /// A platform-dependent character string: ANSI on Windows 98 
                /// and Unicode on Windows NT and Windows XP. This value is only
                /// supported for platform invoke, and not COM interop, because 
                /// exporting a string of type LPTStr is not supported.
                { I.UnmanagedType.LPTStr, "::LPTSTR" },

                /// Used for in-line, fixed-length character arrays that appear 
                /// within a structure. The character type used with ByValTStr 
                /// is determined by the System.Runtime.InteropServices.CharSet 
                /// argument of the 
                /// System.Runtime.InteropServices.StructLayoutAttribute applied
                /// to the containing structure. Always use the 
                /// MarshalAsAttribute.SizeConst field to indicate the size of 
                /// the array. .NET Framework ByValTStr types behave like 
                /// C-style, fixed-size strings inside a structure (for 
                /// example, char s[5]). The behavior in managed code differs 
                /// from the Microsoft Visual Basic 6.0 behavior, which is not 
                /// null terminated (for example, MyString As String * 5).
                { I.UnmanagedType.ByValTStr, "::TCHAR" },

                /// A COM IUnknown pointer. You can use this member on the 
                /// Object data type.
                { I.UnmanagedType.IUnknown, "::IUnknown *" },

                /// A COM IDispatch pointer (Object in Microsoft Visual Basic 
                /// 6.0).
                { I.UnmanagedType.IDispatch, "::IDispatch *" },

                /// A VARIANT, which is used to marshal managed formatted 
                /// classes and value types.
                { I.UnmanagedType.Struct, "" },

                /// A COM interface pointer. The Guid of the interface is 
                /// obtained from the class metadata. Use this member to specify
                /// the exact interface type or the default interface type if 
                /// you apply it to a class. This member produces 
                /// UnmanagedType.IUnknown behavior when you apply it to the 
                /// Object data type.
                { I.UnmanagedType.Interface, "" },

                /// A SafeArray is a self-describing array that carries the 
                /// type, rank, and bounds of the associated array data. You can
                /// use this member with the MarshalAsAttribute.SafeArraySubType 
                /// field to override the default element type.
                { I.UnmanagedType.SafeArray, "::SAFEARRAY *" },

                /// When MarshalAsAttribute.Value is set to ByValArray, the 
                /// SizeConst must be set to indicate the number of elements in 
                /// the array. The ArraySubType field can optionally contain the
                /// UnmanagedType of the array elements when it is necessary to 
                /// differentiate among string types. You can only use this 
                /// UnmanagedType on an array that appear as fields in a 
                /// structure.
                { I.UnmanagedType.ByValArray, "" },

                /// A platform-dependent, signed integer. 4-bytes on 32 bit 
                /// Windows, 8-bytes on 64 bit Windows.
                { I.UnmanagedType.SysInt, "::INT_PTR" },

                /// A platform-dependent, unsigned integer. 4-bytes on 32 bit 
                /// Windows, 8-bytes on 64 bit Windows.
                { I.UnmanagedType.SysUInt, "::UINT_PTR" },

                /// VBByRefStr	Allows Visual Basic 2005 to change a string in 
                /// unmanaged code, and have the results reflected in managed 
                /// code. This value is only supported for platform invoke.
                { I.UnmanagedType.VBByRefStr, "" },

                /// An ANSI character string that is a length prefixed, single
                /// byte. You can use this member on the String data type.
                { I.UnmanagedType.AnsiBStr, "" },

                /// A length-prefixed, platform-dependent char string. ANSI on
                /// Windows 98, Unicode on Windows NT. You rarely use this 
                /// BSTR-like member.
                { I.UnmanagedType.TBStr, "" },

                /// A 2-byte, OLE-defined VARIANT_BOOL type (true = -1, false = 0).
                { I.UnmanagedType.VariantBool, "::VARIANT_BOOL" },

                /// An integer that can be used as a C-style function pointer. 
                /// You can use this member on a Delegate data type or a type 
                /// that inherits from a Delegate.
                { I.UnmanagedType.FunctionPtr, "" },

                /// A dynamic type that determines the type of an object at run 
                /// time and marshals the object as that type. Valid for 
                /// platform invoke methods only.
                { I.UnmanagedType.AsAny, "" },

                /// A pointer to the first element of a C-style array. When 
                /// marshaling from managed to unmanaged, the length of the 
                /// array is determined by the length of the managed array. 
                /// When marshaling from unmanaged to managed, the length of the
                /// array is determined from the MarshalAsAttribute.SizeConst 
                /// and the MarshalAsAttribute.SizeParamIndex fields, optionally
                /// followed by the unmanaged type of the elements within the 
                /// array when it is necessary to differentiate among string 
                /// types.
                { I.UnmanagedType.LPArray, "" },

                /// A pointer to a C-style structure that you use to marshal 
                /// managed formatted classes. Valid for platform invoke methods
                /// only.
                { I.UnmanagedType.LPStruct, "" },

                /// Specifies the custom marshaler class when used with 
                /// MarshalAsAttribute.MarshalType or 
                /// MarshalAsAttribute.MarshalTypeRef. The 
                /// MarshalAsAttribute.MarshalCookie field can be used to pass 
                /// additional information to the custom marshaler. You can use 
                /// this member on any reference type.
                { I.UnmanagedType.CustomMarshaler, "" },

                /// This native type associated with an I4 or a U4 causes the 
                /// parameter to be exported as a HRESULT in the exported type 
                /// library.
                { I.UnmanagedType.Error, "::HRESULT" },
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
