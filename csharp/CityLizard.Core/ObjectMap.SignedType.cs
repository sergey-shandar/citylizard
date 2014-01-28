using System;
using System.IO;
using CityLizard.Policy;

namespace CityLizard.ObjectMap
{
    sealed class SignedType<T, P>: NumberType<T, P>
        where T: struct, IComparable<T>
        where P: struct, ISigned<T>
    {
        public SignedType() : base(NumberCategory.Int)
        {            
        }

        protected override void Serialize(Stream stream, T value)
        {
            Base128.Serialize(ZigZag.Code(new P().ToCommon(value)), stream);
        }
    }
}
