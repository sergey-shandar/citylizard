namespace CityLizard.PInvoke
{
	using R = System.Reflection;
    using I = System.Runtime.InteropServices;

    using System.Linq;
	
	public class CppBuilder
	{
		public string Build(R.Assembly assembly)
		{
            string result = "";
            foreach (var type in assembly.GetTypes())
            {
                foreach (
                    var method in 
                    type.
                        GetMethods().
                        Where(method => (method.Attributes & R.MethodAttributes.PinvokeImpl) != 0))
                {
                    result += 
                        method.ReturnType.ToString() + 
                        " " +
                        method.Name + 
                        "(" + 
                        string.Join(", ", method.GetParameters().Select(p => p.ParameterType.ToString())) + 
                        ");\n";
                }
            }
            return result;
		}
	}
}

