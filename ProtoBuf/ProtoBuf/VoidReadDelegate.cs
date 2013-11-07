using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf
{
    class VoidReadDelegate: IReadDelegate
    {
        public void Variant(ulong value)
        {
        }

        public void Fixed64(double value)
        {
        }

        public void ByteArray(byte[] value)
        {
        }

        public void Fixed32(float value)
        {
        }
    }
}
