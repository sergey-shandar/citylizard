namespace CityLizard.Policy
{
    public struct Base: INumeric<int>, INumeric<long>
    {
        public long Add(long a, long b)
        {
            return a + b;
        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        int INumeric<int>.Zero()
        {
            return 0;
        }

        long INumeric<long>.Zero()
        {
            return 0;
        }

        public static Base X = new Base();
    }
}
