using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityLizard.Policy;

namespace CityLizard.ObjectMap
{
    static class NumberTypeMap
    {
        public static Dictionary<Type, NumberType> Add<T, P>(
            this Dictionary<Type, NumberType> map, NumberType<T, P> value)
            where T : struct, IComparable<T>
            where P : struct, IRange<T>
        {
            map.Add(typeof (T), value);
            return map;
        }

        public static Dictionary<Type, NumberType> AddUnsigned<T, P>(
            this Dictionary<Type, NumberType> map)
            where T: struct, IComparable<T>
            where P: struct, IUnsignedRange<T>
        {
            return map.Add(new UnsignedType<T, P>());
        }

        public static Dictionary<Type, NumberType> AddSigned<T, P>(
            this Dictionary<Type, NumberType> map)
            where T : struct, IComparable<T>
            where P : struct, ISigned<T>
        {
            return map.Add(new SignedType<T, P>());
        }

        public static Dictionary<Type, NumberType> AddFloat<T, P>(
            this Dictionary<Type, NumberType> map)
            where T : struct, IComparable<T>
            where P : struct, IFloat<T>
        {
            return map.Add(new FloatType<T, P>());
        }

        public static Dictionary<Type, NumberType> AddDecimal<T, P>(
            this Dictionary<Type, NumberType> map)
            where T : struct, IComparable<T>
            where P : struct, IDecimal<T>
        {
            return map.Add(new DecimalType<T, P>());
        }

        public static Dictionary<Type, NumberType> Create()
        {
            return
                new Dictionary<Type, NumberType>().
                    // integers
                    AddUnsigned<byte, I>().
                    AddUnsigned<ushort, I>().
                    AddUnsigned<uint, I>().
                    AddUnsigned<ulong, I>().
                    AddUnsigned<bool, I>().
                    AddUnsigned<char, I>().
                    AddSigned<sbyte, I>().
                    AddSigned<short, I>().
                    AddSigned<int, I>().
                    AddSigned<long, I>().
                    AddFloat<float, I>().
                    AddFloat<double, I>().
                    AddDecimal<decimal, I>();
        }
    }
}
