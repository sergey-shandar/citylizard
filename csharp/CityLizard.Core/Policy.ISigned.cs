using System;

namespace CityLizard.Policy
{
    interface ISigned<T>: INumeric<T>, ISignedRange<T> 
        where T : struct, IComparable<T>
    {
    }
}
