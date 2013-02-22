namespace CityLizard.Collections
{
    public abstract class Cache<Key, Data>
    {
        public Data this[Key key]
        {
            get
            {
                var data = this.TryGet(key);
                if (!data.HasValue)
                {
                    data.Value = this.Create(key);
                    this.Set(key, data.Value);
                    this.Initialize(key, data.Value);
                }
                return data.Value;
            }
        }

        protected abstract Optional<Data> TryGet(Key key);

        protected abstract void Set(Key key, Data data);

        protected abstract Data Create(Key key);

        protected virtual void Initialize(Key key, Data data)
        {
        }
    }
}
