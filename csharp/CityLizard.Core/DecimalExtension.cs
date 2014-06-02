using System;
using System.Linq;

namespace CityLizard
{
    static class DecimalExtension
    {
        public static byte[] GetBytes(this decimal value)
        {
            return 
                decimal.
                GetBits(value).
                SelectMany(System.BitConverter.GetBytes).
                ToArray();
        }

        public static decimal ToDecimal(this byte[] bytes)
        {
            var bits = new Int32[4];
            for (int i = 0; i < 4; ++i)
            {
                bits[i] = BitConverter.ToInt32(bytes, i*4);
            }
            return new decimal(bits);
        }
    }
}
