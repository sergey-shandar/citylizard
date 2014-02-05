using System;
using System.Collections.Generic;

namespace CityLizard.Collections
{
    public static class IDictionaryExtension
    {
        public static Optional<T> TryGet<K, T>(
            this IDictionary<K, T> dictionary,
            K key)
        {
            T value;
            return
                dictionary.TryGetValue(key, out value) ?
                    new Optional<T>(value) :
                    null;
        }

        public static T Get<K, T>(
            this IDictionary<K, T> dictionary,
            K key,
            Func<K, T> create)
        {
            var optional = dictionary.TryGet(key);
            if(optional == null)
            {
                var value = create(key);
                dictionary.Add(key, value);
                return value;
            }
            else
            {
                return optional.Value;
            }
        }

    }

}
