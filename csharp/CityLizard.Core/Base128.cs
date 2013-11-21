using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CityLizard
{
    public static class Base128
    {
        private const byte mask = 0x7F;

        private const int offset = 7;

        private const byte flag = 0x80;

        public static void Serialize(ulong value, Stream stream)
        {
            while (true)
            {
                var b = (byte)(value & mask);
                value >>= offset;
                var next = value != 0;
                b |= next ? flag : (byte)0;
                stream.WriteByte(b);
                if (!next)
                {
                    return;
                }
            }
        }

        public static ulong Deserialize(Stream stream)
        {
            var result = 0ul;
            var i = 0;
            while (true)
            {
                var b = (ulong)stream.ReadByte();
                result |= ((b & mask) << i);
                if (b < flag)
                {
                    return result;
                }
                i += offset;
            }
        }

    }
}
