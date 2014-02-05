using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Policy
{
    public interface IBinaryFloat<T>: IFloat<T>
        where T: struct, IComparable<T>
    {
    }
}
