using System;
using System.IO;
using CityLizard.Collections;
using CityLizard.Policy;

namespace CityLizard.ObjectMap
{
    public enum NumberCategory: byte
    {
        Int = 0,
        UInt = 1,
        Float = 2,
        Decimal = 3,
    }

    abstract class NumberType: BaseType
    {
        public readonly NumberCategory NumberCategory;

        public readonly uint Size;

        protected NumberType(NumberCategory numberCategory, uint size):
            base(TypeCategory.Number)
        {
            NumberCategory = numberCategory;
            Size = size;
        }

    }

    abstract class NumberType<T, P> : NumberType
        where T: struct, IComparable<T>
        where P: struct, IRange<T>
    {
        protected NumberType(NumberCategory category):
            base(category, (uint)new P().Size)
        {           
        }

        public sealed override void Serialize(Stream stream, object value)
        {
            Serialize(stream, (T)value);
        }

        protected abstract void Serialize(Stream stream, T value);
    }
}
