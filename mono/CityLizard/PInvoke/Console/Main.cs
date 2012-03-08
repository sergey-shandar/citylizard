using System;

namespace CityLizard.PInvoke.Console
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var builder = new CppBuilder();
            System.Console.Write(builder.Build(typeof(Test.MyClass).Assembly));
		}
	}
}
