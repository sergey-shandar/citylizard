using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Collections
{
    public static class CachedExtension
    {
        public static Func<K, T> Cached<K, T>(
            this Action<K, Action<T>> create)
        {
            var map = new Dictionary<K, T>();
            return k => map.Get(k, create);
        }

        public static Func<T> Cached<T>(this Func<T> get)
        {
            Optional<T> optional = null;
            return () =>
                {
                    if (optional == null)
                    {
                        optional = new Optional<T>(get());
                    }
                    return optional.Value;
                };
        }
    }

}
