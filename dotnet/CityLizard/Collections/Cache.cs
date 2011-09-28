namespace CityLizard.Collections
{
    using G = System.Collections.Generic;

    using Extension;

    public abstract class Cache<Key, Data>
    {
        private G.Dictionary<Key, Data> Dictionary =
            new G.Dictionary<Key, Data>();

        public Data this[Key key]
        {
            get
            {
                var data = this.Dictionary.TryGet(key);
                if (!data.HasValue)
                {
                    data.Value = this.Create(key);
                    this.Dictionary[key] = data.Value;
                    this.Initialize(key, data.Value);
                }
                return data.Value;
            }
        }

        protected abstract Data Create(Key key);

        protected virtual void Initialize(Key key, Data data)
        {
        }
    }
}