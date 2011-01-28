namespace CityLizard.Policy
{
    public interface INumeric<T>
    {
        T Zero();
        T Add(T a, T b);
    }
}
