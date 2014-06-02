using System;

namespace CityLizard.Collections
{
    public struct Optional<T>
    {
        public readonly bool HasValue;

        public readonly T Value;

        public Optional(T value)
        {
            HasValue = true;
            Value = value;
        }

        public T Else(Func<T> elseFunc)
        {
            if (HasValue)
            {
                return Value;
            }
            return elseFunc();
        }
    }
}
