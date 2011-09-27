namespace CityLizard.Collections
{
    public abstract class ArrayCache<Data>
        where Data: class
    {
        private readonly Data[] Array;

        public Data this[int i]
        {
            get
            {
                var result = this.Array[i];
                if (result == null)
                {
                    result = this.Create(i);
                    this.Array[i] = result;
                    this.Initialize(i, result);
                }
                return result;
            }
        }

        public ArrayCache(int size)
        {
            this.Array = new Data[size];
        }

        protected abstract Data Create(int i);

        protected virtual void Initialize(int i, Data data)
        {
        }

    }
}
