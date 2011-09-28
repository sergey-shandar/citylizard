﻿namespace CityLizard.Collections.Extension
{
    using S = System;
    using G = System.Collections.Generic;

    public static class IDictionaryExtension
    {
        public struct Optional<T>
        {
            public bool HasValue;
            public T Value;
        }

        public static Optional<T> TryGet<K, T>(
            this G.IDictionary<K, T> dictionary,
            K key)
        {
            Optional<T> result;
            result.HasValue = dictionary.TryGetValue(key, out result.Value);
            return result;
        }
    }
}
