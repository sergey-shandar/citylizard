using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityLizard.Policy;

namespace CityLizard
{
    interface IFloat<T>: INumeric<T>
        where T: struct, IComparable<T>
    {
    }
}
