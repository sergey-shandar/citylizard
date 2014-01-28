using System;

namespace CityLizard.Policy
{
    interface IDecimal<T>: INumeric<T>
        where T: struct, IComparable<T>
    {
    }
}
