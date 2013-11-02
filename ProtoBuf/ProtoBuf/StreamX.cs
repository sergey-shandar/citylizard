using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    static class StreamX
    {
        public static byte[] ReadByteArray(this Stream stream, int length)
        {
            var result = new byte[length];
            stream.Read(result, 0, length);
            return result;
        }

        public static double ReadDouble(this Stream stream)
        {
            return BitConverter.ToDouble(stream.ReadByteArray(8), 0);
        }

        public static float ReadSingle(this Stream stream)
        {
            return BitConverter.ToSingle(stream.ReadByteArray(4), 0);
        }
    }
}
