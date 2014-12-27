using System;
using System.Collections.Generic;
using Framework.G1;

namespace CityLizard.Collections
{
    public static class IDictionaryExtension
    {
        public static Optional<T> TryGet<K, T>(
            this IDictionary<K, T> dictionary,
            K key)
        {
            T value;
            return dictionary.TryGetValue(key, out value).ThenCreateOptional(value);
        }

        public static T Get<K, T>(
            this IDictionary<K, T> dictionary, K key, Func<T> create)
        {
            return
                dictionary.
                TryGet(key).
                Default(() =>
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
            return dictionary.TryGet(key).Default(() => add(dictionary, key));
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
