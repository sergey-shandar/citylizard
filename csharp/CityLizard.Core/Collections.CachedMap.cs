using System;

namespace CityLizard.Collections
{
    public sealed class CachedMap<K, T>
    {
        private readonly Func<K, T> cached;

        public CachedMap(Func<K, T> create, Action<K, T> init = null)
        {
            cached = create.Cached(init);
        }

        public T this[K key]
        {
            get { return cached(key); }
        }
    }
}
