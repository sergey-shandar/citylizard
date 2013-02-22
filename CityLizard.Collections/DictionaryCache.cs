namespace CityLizard.Collections
{
    using G = System.Collections.Generic;

    using Extension;

    public abstract class DictionaryCache<Key, Data>: Cache<Key, Data>
    {
        private G.Dictionary<Key, Data> Dictionary =
            new G.Dictionary<Key, Data>();

        protected sealed override Optional<Data> TryGet(Key key)
        {
            return this.Dictionary.TryGet(key);
        }

        protected sealed override void Set(Key key, Data data)
        {
            this.Dictionary[key] = data;
        }
    }
}