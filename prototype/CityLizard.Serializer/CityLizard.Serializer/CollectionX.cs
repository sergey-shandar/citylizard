using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLizard.Serializer
{
    internal static class CollectionX
    {
        public static Optional<T> Get<K, T>(
            this IDictionary<K, T> dictionary, K key)
        {
            T value;
            return
                dictionary.TryGetValue(key, out value) ?
                    new Optional<T>(value):
                    null;
        }

        public static T Get<K, T>(
            this IDictionary<K, T> dictionary, K key, Func<K, T> func)
        {
            var optional = dictionary.Get(key);
            return optional != null ? optional.Value : func(key); 
        }
    }
}
