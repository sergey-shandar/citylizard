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
                dictionary.TryGetValue(key, out value)
                    ? new Optional<T>(value)
                    : new Optional<T>();
        }

        public static T Get<K, T>(
            this IDictionary<K, T> dictionary, K key, Func<T> create)
        {
            return
                dictionary.
                TryGet(key).
                Else(() =>
                {
                    var value = create();
                    dictionary.Add(key, value);
                    return value;
                });
        }

        [Obsolete]
        public static T Get<K, T>(
            this IDictionary<K, T> dictionary,
            K key,
            Func<IDictionary<K, T>, K, T> add)
        {
            var optional = dictionary.TryGet(key);
            return optional.HasValue ? optional.Value: add(dictionary, key);
        }

        [Obsolete]
        public static T Get<K, T>(
            this IDictionary<K, T> dictionary,
            K key,
            Func<K, T> create)
        {
            return dictionary.Get(
                key,
                (d, k) =>
                {
                    var value = create(k);
                    d.Add(k, value);
                    return value;
                });
        }

    }

}
