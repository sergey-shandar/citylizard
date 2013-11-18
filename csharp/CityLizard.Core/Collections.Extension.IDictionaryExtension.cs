namespace CityLizard.Collections.Extension
{
    using S = System;
    using G = System.Collections.Generic;

    public static class IDictionaryExtension
    {
        public static Optional<T> TryGet<K, T>(
            this G.IDictionary<K, T> dictionary,
            K key)
        {
            T value;
            return
                dictionary.TryGetValue(key, out value) ?
                    new Optional<T>(value) :
                    null;
        }
    }
}
