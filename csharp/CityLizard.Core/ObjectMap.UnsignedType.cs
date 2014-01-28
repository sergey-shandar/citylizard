using System;
using System.IO;
using CityLizard.Policy;

namespace CityLizard.ObjectMap
{
    sealed class UnsignedType<T, P>: NumberType<T, P>
        where T: struct, IComparable<T>
        where P: struct, IUnsignedRange<T>
    {
        public UnsignedType(): base(NumberCategory.UInt)
        {        
        }

        protected override void Serialize(Stream stream, T value)
        {
            Base128.Serialize(new P().ToCommon(value), stream);
        }
    }
}
