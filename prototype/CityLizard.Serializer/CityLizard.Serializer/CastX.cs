using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CityLizard.Serializer
{
    public static class CastX
    {
        public static T UpCast<T>(this T value)
        {
            return value;
        }
    }
}
