namespace CityLizard.Collections
{
    public abstract class IdCache<Key, Data> : Cache<Key, Data>
    {
        private int Value;

        private int GetNew()
        {
            var value = this.Value;
            ++this.Value;
            return value;
        }

        protected abstract Data Create(Key key, int id);

        protected sealed override Data Create(Key key)
        {
            return this.Create(key, this.GetNew());
        }
    }
}
