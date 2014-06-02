using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Collections
{
    public static class CachedExtension
    {
        public static Func<T> Cached<T>(this Func<T> get)
        {
            Func<T> func;
            func = () =>
            {
                var result = get();
                func = () => result;
                return result;
            };
            return () => func();
        }

        public static Func<K, T> Cached<K, T>(
            this IDictionary<K, T> map, Func<K, T> create)
        {
            return k => map.Get(k, create);
        }

        public static Func<K, T> Cached<K, T>(this Func<K, T> create)
        {
            return new Dictionary<K, T>().Cached(create);
        }

        public static Func<K, Func<T>> CachedCached<K, T>(
            this Func<K, T> create)
        {
            return Cached((K k) => Cached(() => create(k)));
        }
    }

}
