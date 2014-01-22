using System;
using System.Collections.Generic;
using System.Linq;

using CityLizard.Collections;

namespace CityLizard.ObjectMap
{
    public sealed class ObjectMap
    {
        private readonly Dictionary<Type, Func<Action<BaseType>, NumberType>> 
            _numberTypeMap =
                new Dictionary<Type, Func<Action<BaseType>, NumberType>>();

        private void AddNumberType<T>(NumberCategory category, byte expSize)
            where T: struct
        {
            _numberTypeMap.Add(
                typeof(T), r => new NumberType(r, category, expSize));
        }

        private void AddIntType<S, U>(byte expSize)
            where S: struct
            where U: struct
        {
            AddNumberType<S>(NumberCategory.Int, expSize);
            AddNumberType<U>(NumberCategory.UInt, expSize);
        }

        private readonly CachedMap<Type, BaseType> _typeMap;

        private Func<BaseType> Cast(Type type)
        {
            return () => _typeMap[type];
        }

        private Field[] ToFieldArray(Type type)
        {
            return 
                type.
                GetFields().
                Select(field => new Field(field.Name, Cast(field.FieldType))).
                ToArray();
        }

        private readonly CachedMap<Object, ulong> _map;

        public ObjectMap()
        {
            // integers
            AddIntType<sbyte, byte>(0);
            AddIntType<short, ushort>(1);
            AddIntType<int, uint>(2);
            AddIntType<long, ulong>(3);
            
            // additional integer types
            AddNumberType<bool>(NumberCategory.UInt, 0);
            AddNumberType<char>(NumberCategory.UInt, 1);

            // binary floating point numbers (32 and 64 bit)
            AddNumberType<float>(NumberCategory.Float, 2);
            AddNumberType<double>(NumberCategory.Float, 3);
            
            // decimal floating point number (128 bit)
            AddNumberType<decimal>(NumberCategory.Decimal, 4);
            
            // System.Type to BaseType mapping.
            _typeMap = new CachedMap<Type, BaseType>(
                (k, register) =>
                {
                    if (k.IsByRef)
                    {
                        if (k.IsArray)
                        {
                            new ArrayType(
                                register,
                                (byte)k.GetArrayRank(), 
                                Cast(k.GetElementType()));
                        }
                        else if (k == typeof(string))
                        {
                            new ArrayType(register, 1, Cast(typeof(Char)));
                        }
                        else
                        {
                            new ClassType(
                                register,
                                k.Name,
                                Cast(k.BaseType),
                                ToFieldArray(k));
                        }
                    }
                    else if(k.IsPrimitive)
                    {
                        _numberTypeMap[k](register);
                    }
                    else if(k.IsEnum)
                    {
                        _numberTypeMap[Enum.GetUnderlyingType(k)](register);
                    }
                    else
                    {
                        new StructType(register, k.Name, ToFieldArray(k));
                    }
                });

            //
            _map = IdMap.Create<object>((o, i) => {});
        }

        /*
        private readonly Dictionary<Object, ulong> Map =
            new Dictionary<object, ulong>();

        private static BaseType Cast(Type type)
        {
            if(type.IsByRef)
            {
                if(type.IsArray)
                {
                    return new ArrayType(
                        (byte)type.GetArrayRank(), Cast(type.GetElementType()));
                }
                else if(type == typeof(string))
                {
                    return new ArrayType(1, Cast(typeof(Char)));
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private ulong SerializeType(Type type)
        {
            Contract.Requires(type != null);
            return 0;
        }

        public ulong Serialize(ulong typeId, Object value)
        {
            Contract.Requires(value != null);
            // TODO
            return 0;
        }

        public ulong Serialize(Object value)
        {
            // null is always serialized as 0.
            if (value == null)
            {
                return 0;
            }
            else 
            {
                var idRef = Map.TryGet(value);
                // check if the value already exist.
                if (idRef != null)
                {
                    return idRef.Value;
                }
                // serialize the value.
                else       
                {
                    // serialize type.
                    var type = value.GetType();
                    if (type == typeof(Type))
                    {
                        return SerializeType((Type)value);
                    }
                    else
                    {
                        return Serialize(SerializeType(type), value);
                    }
                }
            }
        }
         * */
    }
}
