namespace CityLizard.Collections
{
    public sealed class Optional<T>
    {
        public readonly T Value;

        public Optional(T value)
        {
            Value = value;
        }
    }
}
