namespace CityLizard.Collections
{
    public class Optional<T>
    {
        public readonly T Value;

        public Optional(T value)
        {
            Value = value;
        }
    }
}
