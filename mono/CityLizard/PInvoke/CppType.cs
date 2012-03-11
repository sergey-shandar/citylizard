namespace CityLizard.PInvoke
{
    using S = System;
    using I = System.Runtime.InteropServices;
    using R = System.Reflection;

    using System.Linq;

    static class CppType
    {
        private const string Bool = "::BOOL";
        
        private const string BStr = "::BSTR";

        private const string Char = "::CHAR";
        private const string Short = "::SHORT";
        private const string Long = "::LONG";
        private const string LongLong = "::LONGLONG";

        private const string Byte = "::BYTE";
        private const string UShort = "::USHORT";
        private const string ULong = "::ULONG";
        private const string ULongLong = "::ULONGLONG";

        private const string Float = "::FLOAT";
        private const string Double = "::DOUBLE";

        private const string IntPtr = "::INT_PTR";
        private const string UIntPtr = "::UINT_PTR";

        private const string SafeArrayPtr = "::SAFEARRAY *";

        public const string HResult = "::HRESULT";

        public static string ToCppType(this S.Type type, R.ICustomAttributeProvider provider)
        {
            if (type.IsByRef)
            {
                type = type.GetElementType();
            }
            var marshalAs = provider.GetCustomAttribute<I.MarshalAsAttribute>(true);
            if (marshalAs != null)
            {
                switch (marshalAs.Value)
                {
                    case I.UnmanagedType.AsAny:
                        return "::VARIANT";
                    case I.UnmanagedType.Bool:
                        return Bool;
                    case I.UnmanagedType.BStr:
                        return BStr;
                    case I.UnmanagedType.Currency:
                        return "::CY";
                    case I.UnmanagedType.Error:
                        return HResult;
                    case I.UnmanagedType.I1:
                        return Char;
                    case I.UnmanagedType.I2:
                        return Short;
                    case I.UnmanagedType.I4:
                        return Long;
                    case I.UnmanagedType.I8:
                        return LongLong;
                    case I.UnmanagedType.IDispatch:
                        return "::IDispatch *";
                    case I.UnmanagedType.IUnknown:
                        return "::IUnknown *";
                    case I.UnmanagedType.LPStr:
                        return "::LPSTR";
                    case I.UnmanagedType.LPTStr:
                        return "::LPTSTR";
                    case I.UnmanagedType.LPWStr:
                        return "::LPWSTR";
                    case I.UnmanagedType.R4:
                        return Float;
                    case I.UnmanagedType.R8:
                        return Double;
                    case I.UnmanagedType.SafeArray:
                        return SafeArrayPtr;
                    case I.UnmanagedType.SysInt:
                        return IntPtr;
                    case I.UnmanagedType.SysUInt:
                        return UIntPtr;
                    case I.UnmanagedType.U1:
                        return Byte;
                    case I.UnmanagedType.U2:
                        return UShort;
                    case I.UnmanagedType.U4:
                        return ULong;
                    case I.UnmanagedType.U8:
                        return ULongLong;
                    case I.UnmanagedType.VariantBool:
                        return "::VARIANT_BOOL";
                }
            }
            
            if (type == typeof(void))
            {
                return "void";
            }

            var name = "::" + type.ToString().Replace(".", "::");

            if (type.IsEnum)
            {
                return name + "::value_type";
            }

            if (type.IsValueType)
            {
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
                //
                // Marshaling byval C-structure as return value in C#
                // http://stackoverflow.com/questions/4845128/marshaling-byval-c-structure-as-return-value-in-c-sharp
                if (type == typeof(bool))
                {
                    return Bool;
                }
                else if (type == typeof(char))
                {
                    return "::WCHAR";
                }
                else if (type == typeof(string))
                {
                    return BStr;
                }
                else if (type == typeof(sbyte))
                {
                    return Char;
                }
                else if (type == typeof(short))
                {
                    return Short;
                }
                else if (type == typeof(int))
                {
                    return Long;
                }
                else if (type == typeof(long))
                {
                    return LongLong;
                }
                else if (type == typeof(byte))
                {
                    return Byte;
                }
                else if (type == typeof(ushort))
                {
                    return UShort;
                }
                else if (type == typeof(uint))
                {
                    return ULong;
                }
                else if (type == typeof(ulong))
                {
                    return ULongLong;
                }
                else if (type == typeof(float))
                {
                    return Float;
                }
                else if (type == typeof(double))
                {
                    return Double;
                }
                else if (type == typeof(S.IntPtr))
                {
                    return IntPtr;
                }
                else if (type == typeof(S.UIntPtr))
                {
                    return UIntPtr;
                }
                else
                {
                    return name;
                }
            }

            if (type.IsArray)
            {
                return SafeArrayPtr;
            }

            throw new S.Exception("unknown type");
        }

        public static string ToCppType(this R.ParameterInfo p)
        {
            return p.ParameterType.ToCppType(p);
        }

        public static string ToCppType(this R.FieldInfo f)
        {
            return f.FieldType.ToCppType(f);
        }
    }
}
