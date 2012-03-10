using System;

namespace CityLizard.PInvoke.Console
{
    using IO = System.IO;

	class MainClass
	{
		public static void Main (string[] args)
		{
			var builder = new CppBuilder();
            var sb = builder.Build(typeof(Test.MyClass).Assembly);
            using (var outfile = new IO.StreamWriter("../../../../../../citylizard_pinvoke_test_cpp/interface.hpp"))
            {
                outfile.Write(sb.ToString());
            }
		}
	}
}
