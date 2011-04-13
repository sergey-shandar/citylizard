namespace CityLizard.Tree
{
    class RedBlack<T>
    {
        private struct Data
        {
            public bool Red;
            public T Value;
        }

        private Base<Data> Base;
    }
}
