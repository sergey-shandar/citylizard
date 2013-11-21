using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard
{
    public static class ZigZag
    {
        public static ulong Code(long value)
        {
            return ((ulong)value << 1) ^ (ulong)(value >> 63);
        }

        public static long Decode(ulong zigZag)
        {
            return (long)(zigZag >> 1) ^ ((long)(zigZag << 63) >> 63);
        }

    }
}
