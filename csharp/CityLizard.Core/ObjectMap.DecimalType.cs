using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityLizard.Policy;

namespace CityLizard.ObjectMap
{
    sealed class DecimalType<T, P>: BaseFloatType<T, P>
        where T: struct, IComparable<T>
        where P: struct, INumeric<T>
    {
        public DecimalType() : base(NumberCategory.Decimal)
        { 
        }
    }
}
