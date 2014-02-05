using System;

namespace CityLizard.Policy
{
    interface IDecimalFloat<T>: IFloat<T>
        where T: struct, IComparable<T>
    {
    }
}
