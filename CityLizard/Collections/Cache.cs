namespace CityLizard.Collections
{
    using G = System.Collections.Generic;

    public abstract class Cache<Key, Data>
    {
        private G.Dictionary<Key, Data> Dictionary =
            new G.Dictionary<Key, Data>();

        public Data this[Key key]
        {
            get
            {
                Data data;
                if (!this.Dictionary.TryGetValue(key, out data))
                {
                    data = this.Create(key);
                    this.Dictionary[key] = data;
                    this.Initialize(key, data);
                }
                return data;
            }
        }

        protected abstract Data Create(Key key);

        protected virtual void Initialize(Key key, Data data)
        {
        }
    }
}