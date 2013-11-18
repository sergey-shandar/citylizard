namespace CityLizard.Collections
{
    public abstract class Cache<Key, Data>
    {
        public Data this[Key key]
        {
            get
            {
                var data = this.TryGet(key);
                if (data == null)
                {
                    var value = this.Create(key);
                    this.Set(key, value);
                    this.Initialize(key, value);
                    return value;
                }
                else
                {
                    return data.Value;
                }
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
