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
    }
}
