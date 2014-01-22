using System;

namespace CityLizard.Collections
{
    public sealed class CachedMap<K, T>
    {
        private readonly Func<K, T> cached;

        public CachedMap(Action<K, Action<T>> create)
        {
            cached = create.Cached();
        }

        public T this[K key]
        {
            get { return cached(key); }
        }
    }
}
