namespace CityLizard.Policy
{
    public interface INumeric<T>: IRange<T> 
        where T: struct, System.IComparable<T>
    {
        T _0 { get; }
        T _1 { get; }
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);
    }
}
