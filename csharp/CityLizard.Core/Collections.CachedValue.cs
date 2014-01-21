using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Collections
{
    public sealed class CachedValue<T>
    {
        readonly Func<T> Func;

        public T Value
        {
            get { return Func(); }
        }

        public CachedValue(Func<T> func)
        {
            Func = func.Cached();
        }
    }
}
