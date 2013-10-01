using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace builder
{
    sealed class CppFile
    {
        public readonly string Name;

        public readonly string Condition;

        public CppFile(string name, string condition = null)
        {
            Name = name;
            Condition = condition;
        }

        public IEnumerable<string> Code
        {
            get
            {
                var include = "#include \"" + Name + "\"";
                if (Condition == null)
                {
                    yield return include;
                }
                else
                {
                    yield return "#if " + Condition;
                    yield return include;
                    yield return "#endif";
                }
            }
        }
    }
}
