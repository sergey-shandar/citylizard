using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Collections
{
    public static class CachedValueExtension
    {
        public static CachedValue<T> CachedValue<T>(this Func<T> func)
        {
            return new CachedValue<T>(func);
        }
    }
}
