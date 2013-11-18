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
    }
}
