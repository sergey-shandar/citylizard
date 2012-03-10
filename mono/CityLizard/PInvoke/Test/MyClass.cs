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

        /*
		[I.DllImport("citylizard_pinvoke_test_cpp.dll")]
		public static extern void A(MyStruct s);

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern int C();

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern int D(int a, int b, char f);
		
		[I.DllImport("citylizard_pinvoke_test_cpp.dll")]
		public static extern IMyInterface GetInterface(string x);

        public static void B() { }
         * */
	}
}

