namespace CityLizard.PInvoke.Test
{
	using I = System.Runtime.InteropServices;	
	
	public static class MyClass
	{
        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern int A();

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false, CallingConvention = I.CallingConvention.Cdecl)]
        public static extern int B();

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void C();

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern int D(byte a, short c);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern MyEnum E(MyEnum a, MyEnum b);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", CallingConvention = I.CallingConvention.Cdecl)]
        public static extern MyBigEnum BigE(MyBigEnum a, MyBigEnum b);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern MyPBigEnum PBigE(MyPBigEnum a, MyPBigEnum b);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", CallingConvention = I.CallingConvention.StdCall)]
        public static extern IntEnum IntE(UIntEnum a, UIntEnum b);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern bool R(int x);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckBool(bool a, bool b, bool c, bool d, int x);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckBool2(
            [I.MarshalAs(I.UnmanagedType.I1)] bool a,
            [I.MarshalAs(I.UnmanagedType.I1)] bool b,
            [I.MarshalAs(I.UnmanagedType.I1)] bool c,
            [I.MarshalAs(I.UnmanagedType.I1)] bool d, 
            int x);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckBool3(
            [I.MarshalAs(I.UnmanagedType.VariantBool)] bool a,
            [I.MarshalAs(I.UnmanagedType.VariantBool)] bool b,
            [I.MarshalAs(I.UnmanagedType.VariantBool)] bool c,
            [I.MarshalAs(I.UnmanagedType.VariantBool)] bool d,
            int x);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern bool RetBool();

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        [return: I.MarshalAs(I.UnmanagedType.VariantBool)]
        public static extern bool RetBool3();

        // // does not work: "Method's type signature is not PInvoke compatible."
        // [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        // public static extern MyStruct RetStructP();

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern MyStruct RetStruct();

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void SetStruct(MyStruct s);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void SetStructBool(MyStructBool s);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckBools(MyBools y, int x);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckBools2(MyBools2 y, int x);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckBools3(MyBools3 y, int x);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern void RetBoolOut(out MyBools3 result);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void PackTest(NoPack noPackm, Pack1 pack1);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void PrivateStruct(Private p);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void StringStruct(String s);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void StringStructAnsi(StringAnsi s);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckBStr(BStr s);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void MyLPTStr([I.MarshalAs(I.UnmanagedType.LPTStr)] string x);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false, CharSet = I.CharSet.Ansi)]
        public static extern void AnsiMyLPTStr(
            [I.MarshalAs(I.UnmanagedType.LPTStr)] string x,
            [I.MarshalAs(I.UnmanagedType.BStr)]string y);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckChars(Chars chars);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckAnsiChars(AnsiChars chars);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll", PreserveSig = false)]
        public static extern void CheckArray(ByValArray x);
	}
}

