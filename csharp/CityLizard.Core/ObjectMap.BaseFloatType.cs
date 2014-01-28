using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CityLizard.Policy;

namespace CityLizard.ObjectMap
{
    abstract class BaseFloatType<T, P>: NumberType<T, P>
        where T: struct, IComparable<T>
        where P: struct, INumeric<T>
    {
        protected BaseFloatType(NumberCategory category) : base(category)
        { 
        }

        protected sealed override void Serialize(Stream stream, T value)
        {
            stream.Write(new P().GetBytes(value));
        }
    }
}
