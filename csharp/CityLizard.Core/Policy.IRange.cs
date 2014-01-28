using System;

namespace CityLizard.Policy
{
    public interface IRange<T>
        where T: struct, IComparable<T>
    {
        int Size { get; }
        byte[] GetBytes(T value);
        T MinValue { get; }
        T MaxValue { get; }
    }
}
