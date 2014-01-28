using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityLizard.Collections;
using CityLizard.Functional;
using CityLizard.Xml;

namespace CityLizard.ObjectMap
{
    static class TypeMap
    {
        private static Func<BaseType> Cast(
            this Func<Type, BaseType> map, Type type)
        {
            return () => map(type);
        }

        private static Field[] ToFieldArray(
            this Func<Type, BaseType> map, Type type)
        {
            return
                type.
                GetFields().
                Where(field => !field.IsStatic).
                Select(
                    field => new Field(field.Name, map.Cast(field.FieldType))).
                ToArray();
        }

        public static Func<Type, BaseType> Create()
        {
            var numberTypeMap = NumberTypeMap.Create();
            return Extension.UseResult<Func<Type, BaseType>>(getMap =>
                CachedExtension.Cached<Type, BaseType>(
                    k =>
                    {
                        var map = getMap();
                        if (k.IsByRef)
                        {
                            if (k.IsArray)
                            {
                                return new ArrayType(
                                    (byte)k.GetArrayRank(),
                                    map.Cast(k.GetElementType()));
                            }
                            else if (k == typeof(string))
                            {
                                return new ArrayType(
                                    1, getMap().Cast(typeof(Char)));
                            }
                            else
                            {
                                return new ClassType(
                                    k.Name,
                                    map.Cast(k.BaseType),
                                    map.ToFieldArray(k));
                            }
                        }
                        else if (k.IsPrimitive)
                        {
                            return numberTypeMap[k];
                        }
                        else if (k.IsEnum)
                        {
                            return numberTypeMap[Enum.GetUnderlyingType(k)];
                        }
                        else
                        {
                            return new StructType(k.Name, map.ToFieldArray(k));
                        }
                    }));
        }
    }
}
