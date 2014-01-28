using System;

namespace CityLizard.Policy
{
    interface IUnsigned<T>: INumeric<T>, IUnsignedRange<T> 
        where T: struct, IComparable<T>
    {
    }
}
