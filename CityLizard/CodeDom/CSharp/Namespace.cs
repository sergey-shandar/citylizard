using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.CodeDom.CSharp
{
    /// <summary>
    /// C# namespace name.
    /// </summary>
    public static class Namespace
    {
        private const string Http = "http://";

        /// <summary>
        /// Converts text to valid C# namespace name.
        /// </summary>
        /// <param name="s">Text.</param>
        /// <returns>Valid C# namespace name.</returns>
        public static string Cast(string s)
        {
            if (s.StartsWith(Http))
            {
                s = s.Remove(0, Http.Length);
            }
            var names = s.Split('/');
            var result = "";
            foreach (var n in names)
            {
                if (n != string.Empty)
                {
                    var newName = Name.Cast(n);
                    if (result == string.Empty)
                    {
                        result = newName;
                    }
                    else
                    {
                        result += "." + newName;
                    }
                }
            }
            return result;
        }
    }
}
