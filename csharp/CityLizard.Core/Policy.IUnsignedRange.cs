using System;

namespace CityLizard.Policy
{
    interface IUnsignedRange<T>: IRange<T>
        where T: struct, IComparable<T>
    {
        ulong ToCommon(T value);
        T FromCommon(ulong value);
    }
}
