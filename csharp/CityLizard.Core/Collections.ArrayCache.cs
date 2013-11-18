namespace CityLizard.Collections
{
    public abstract class ArrayCache<Data>: Cache<int, Data>
        where Data: class
    {
        private readonly Data[] Array;

        public ArrayCache(int size)
        {
            this.Array = new Data[size];
        }

        protected sealed override Optional<Data> TryGet(int key)
        {
            var data = this.Array[key];
            return data == null ? null: new Optional<Data>(data); 
        }

        protected sealed override void Set(int key, Data data)
        {
            this.Array[key] = data;
        }

    }
}
