namespace CityLizard.Policy
{
    public interface INumeric<T>
    {
        T _0();
        T _1();
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);
    }
}
