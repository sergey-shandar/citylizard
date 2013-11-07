using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf
{
    public class TypeMismatch: PBException
    {
        public TypeMismatch(WireType expected, WireType actual):
            base("type mismatch: expected: " + expected + ", actual: " + actual)
        {
        }
    }
}
