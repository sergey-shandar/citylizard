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
        private const string LPStr = "::LPSTR";
        private const string LPWStr = "::LPWSTR";
        private const string LPTStr = "::LPTSTR";

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

        public struct Type
        {
            public readonly string Prefix;
            public readonly string Suffix;

            public Type(string prefix, string suffix = "")
            {
                this.Prefix = prefix;
                this.Suffix = suffix;
            }
        }

        private static string CharType(I.CharSet charSet)
        {
            switch(charSet)
            {
                case I.CharSet.Ansi:
                    return Char;
                case I.CharSet.Auto:
                    return "::TCHAR";
                case I.CharSet.Unicode:
                    return "::WCHAR";
                default:
                    throw new S.Exception("unknown charser: " + charSet);
            }
        }

        private static string StringType(I.CharSet charSet)
        {
            switch (charSet)
            {
                case I.CharSet.Ansi:
                    return LPStr;
                case I.CharSet.Auto:
                    return LPTStr;
                case I.CharSet.Unicode:
                    return LPWStr;
                default:
                    throw new S.Exception("unknown charser: " + charSet);
            }
        }

        public static Type ToCppType(this S.Type type, R.ICustomAttributeProvider provider, I.CharSet charSet)
        {
            if (type.IsByRef)
            {
                type = type.GetElementType();
            }
            var marshalAs = provider.GetCustomAttribute<I.MarshalAsAttribute>(true);
            if (marshalAs != null)
            {
                var unmanagedType = marshalAs.Value;
                switch (unmanagedType)
                {
                    case I.UnmanagedType.AsAny:
                        return new Type("::VARIANT");
                    case I.UnmanagedType.Bool:
                        return new Type(Bool);
                    case I.UnmanagedType.BStr:
                        return new Type(BStr);
                    case I.UnmanagedType.Currency:
                        return new Type("::CY");
                    case I.UnmanagedType.Error:
                        return new Type(HResult);
                    case I.UnmanagedType.I1:
                        return new Type(Char);
                    case I.UnmanagedType.I2:
                        return new Type(Short);
                    case I.UnmanagedType.I4:
                        return new Type(Long);
                    case I.UnmanagedType.I8:
                        return new Type(LongLong);
                    case I.UnmanagedType.IDispatch:
                        return new Type("::IDispatch *");
                    case I.UnmanagedType.IUnknown:
                        return new Type("::IUnknown *");
                    case I.UnmanagedType.LPStr:
                        return new Type(LPStr);
                    // depends on platform
                    case I.UnmanagedType.LPTStr:
                        return new Type(LPTStr);
                    case I.UnmanagedType.LPWStr:
                        return new Type(LPWStr);
                    case I.UnmanagedType.R4:
                        return new Type(Float);
                    case I.UnmanagedType.R8:
                        return new Type(Double);
                    case I.UnmanagedType.SafeArray:
                        return new Type(SafeArrayPtr);
                    case I.UnmanagedType.SysInt:
                        return new Type(IntPtr);
                    case I.UnmanagedType.SysUInt:
                        return new Type(UIntPtr);
                    case I.UnmanagedType.U1:
                        return new Type(Byte);
                    case I.UnmanagedType.U2:
                        return new Type(UShort);
                    case I.UnmanagedType.U4:
                        return new Type(ULong);
                    case I.UnmanagedType.U8:
                        return new Type(ULongLong);
                    case I.UnmanagedType.VariantBool:
                        return new Type("::VARIANT_BOOL");
                    // depends on charset configuratin
                    case I.UnmanagedType.ByValTStr:
                        return new Type(CharType(charSet), "[" + marshalAs.SizeConst + "]");
                    default:
                        throw new S.Exception("Unsupported native type: " + unmanagedType);
                }
            }
            
            if (type == typeof(void))
            {
                return new Type("void");
            }

            var name = "::" + type.ToString().Replace(".", "::");

            if (type.IsEnum)
            {
                return new Type(name + "::value_type");
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
                    return new Type(Bool);
                }
                else if (type == typeof(char))
                {
                    return new Type(CharType(charSet));
                }
                else if (type == typeof(sbyte))
                {
                    return new Type(Char);
                }
                else if (type == typeof(short))
                {
                    return new Type(Short);
                }
                else if (type == typeof(int))
                {
                    return new Type(Long);
                }
                else if (type == typeof(long))
                {
                    return new Type(LongLong);
                }
                else if (type == typeof(byte))
                {
                    return new Type(Byte);
                }
                else if (type == typeof(ushort))
                {
                    return new Type(UShort);
                }
                else if (type == typeof(uint))
                {
                    return new Type(ULong);
                }
                else if (type == typeof(ulong))
                {
                    return new Type(ULongLong);
                }
                else if (type == typeof(float))
                {
                    return new Type(Float);
                }
                else if (type == typeof(double))
                {
                    return new Type(Double);
                }
                else if (type == typeof(S.IntPtr))
                {
                    return new Type(IntPtr);
                }
                else if (type == typeof(S.UIntPtr))
                {
                    return new Type(UIntPtr);
                }
                else
                {
                    return new Type(name);
                }
            }

            if (type == typeof(string))
            {
                return new Type(charSet == I.CharSet.Ansi ? LPStr: LPWStr);
            }

            if (type.IsArray)
            {
                return new Type(SafeArrayPtr);
            }

            throw new S.Exception("unsupported type: " + type);
        }

        public static Type ToCppType(this R.ParameterInfo p)
        {
            return p.ParameterType.ToCppType(p, p.Member.GetCustomAttribute<I.DllImportAttribute>(true).CharSet);
        }

        public static Type ToCppType(this R.FieldInfo f)
        {
            return f.FieldType.ToCppType(f, f.DeclaringType.StructLayoutAttribute.CharSet);
        }
    }
}
