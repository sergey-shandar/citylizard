namespace CityLizard.PInvoke.Test
{
	using I = System.Runtime.InteropServices;
	
	public static class MyClass
	{
		[I.DllImport("citylizard_pinvoke_test_cpp.dll")]
		public static extern void A();

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern int C();

        [I.DllImport("citylizard_pinvoke_test_cpp.dll")]
        public static extern int D(int a, int b, char f);

        public static void B() { }
	}
}

