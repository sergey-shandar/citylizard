using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard
{
    public static class EncodingExtension
    {
        public static string GetString(this Encoding encoding, byte[] array)
        {
            return encoding.GetString(array, 0, array.Length);
        }
    }
}
