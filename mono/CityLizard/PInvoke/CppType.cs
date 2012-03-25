namespace CityLizard.PInvoke
{
    using S = System;
    using I = System.Runtime.InteropServices;
    using R = System.Reflection;
    using C = System.Collections.Generic;

    using System.Linq;

    static class CppType
    {
        private class Map
        {
            private class Value
            {
                private readonly C.Dictionary<I.UnmanagedType?, string> dictionary =
                    new C.Dictionary<I.UnmanagedType?, string>();

                public Value Add(I.UnmanagedType? unmanagedType, string value)
                {
                    this.dictionary.Add(unmanagedType, value);
                    return this;
                }
            }

            private readonly C.Dictionary<S.Type, Value> dictionary =
                new C.Dictionary<S.Type, Value>();

            public Map()
            {
                this.AddOptional<sbyte>(I.UnmanagedType.I1, "::CHAR");
                this.AddOptional<short>(I.UnmanagedType.I2, "::SHORT");
                this.AddOptional<int>(I.UnmanagedType.I4, "::LONG");
                this.AddOptional<long>(I.UnmanagedType.I8, "::LONGLONG");
                this.AddOptional<byte>(I.UnmanagedType.U1, "::BYTE");
                this.AddOptional<ushort>(I.UnmanagedType.U2, "::USHORT");
                this.AddOptional<uint>(I.UnmanagedType.U4, "::ULONG");
                this.AddOptional<ulong>(I.UnmanagedType.U8, "::ULONGLONG");
                this.AddOptional<S.IntPtr>(I.UnmanagedType.SysInt, "::INT_PTR");
                this.AddOptional<S.UIntPtr>(I.UnmanagedType.SysUInt, "::UINT_PTR");
            }

            private Value Add<T>()
            {
                var value = new Value();
                this.dictionary.Add(typeof(T), value);
                return value;
            }

            private void AddOptional<T>(I.UnmanagedType unmanagedType, string cType)
            {
                var value = this.Add<T>();
                value.Add(null, cType);
                value.Add(unmanagedType, cType);
            }
        }

        /*
        private static C.Dictionary<S.Type, string> dictonary = 
            new C.Dictionary<S.Type, string>()
        {
            // integers
            { typeof(sbyte), "::CHAR" },
            { typeof(short), "::SHORT" },
            { typeof(int), "::LONG" },
            { typeof(long), "::LONGLONG" },
            { typeof(byte), "::BYTE" },
            { typeof(ushort), "::USHORT" },
            { typeof(uint), "::ULONG" },
            { typeof(ulong), "ULONGLONG" },

            // system integers
            { typeof(S.IntPtr), "INT_PTR" },
            { typeof(S.UIntPtr), "UINT_PTR" },

            // floating-point numbers
            { typeof(float), "::FLOAT" },
            { typeof(double), "::DOUBLE" },
        };

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

        public static Type ToCppType(this S.Type type, R.ICustomAttributeProvider provider,
            I.CharSet charSet)
        {
            var marshalAs = provider.GetCustomAttribute<I.MarshalAsAttribute>(true);
            string value;
            if (dictonary.TryGetValue(type, out value))
            {
                if (marshalAs != null)
                {
                    throw new S.Exception("MarshalAs attribute is not supported for " + type.ToString());
                }
                return new Type(value, "");
            }
            else if(type == typeof(bool))
            {
                if (marshalAs == null)
                {
                    return new Type("::BOOL", "");
                }
                else
                {
                    switch (marshalAs.Value)
                    {
                        case I.UnmanagedType.Bool:
                            return new Type("::BOOL", "");
                        case I.UnmanagedType.VariantBool:
                            return new Type("::VARIANT_BOOL", "");
                        default:
                            throw new S.Exception("Can't marshal bool as " + marshalAs.Value.ToString());
                    }
                }
            }
            else if (type == typeof(object))
            {
            }
        }
        */

        /*
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

        public static string ToCppType(I.UnmanagedType unmanagedType, I.CharSet charSet)
        {
            switch (unmanagedType)
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
                    return LPStr;
                // depends on platform
                case I.UnmanagedType.LPTStr:
                    return LPTStr;
                case I.UnmanagedType.LPWStr:
                    return LPWStr;
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
                default:
                    throw new S.Exception("Unsupported native type: " + unmanagedType);
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
                    // depends on charset configuratin
                    case I.UnmanagedType.ByValTStr:
                        return new Type(CharType(charSet), "[" + marshalAs.SizeConst + "]");
                    case I.UnmanagedType.ByValArray:
                        return new Type(ToCppType(marshalAs.ArraySubType, charSet), "[" + marshalAs.SizeConst + "]");
                    default:
                        return new Type(ToCppType(unmanagedType, charSet), "");
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
         * */

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
