using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Policy
{
    interface ISignedRange<T>: IRange<T>
        where T: struct, IComparable<T>
    {
        long ToCommon(T value);
        T FromCommon(long value);
    }
}
