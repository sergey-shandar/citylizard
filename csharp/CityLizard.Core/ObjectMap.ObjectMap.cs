using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using CityLizard.Collections;

namespace CityLizard.ObjectMap
{
    public sealed class ObjectMap
    {
        private static void Serialize(
            Func<object, ulong> idMap,
            Stream stream,
            // BaseType type,
            object value)
        {
            // var type = value.GetType();
            // TODO:
        }

        public static void Serialize(object value, Stream stream)
        {
            if (value == null)
            {
                return;
            }
            var typeMap = TypeMap.Create();
            ulong i = 0;
            var queue = new Queue<object>();
            var idMap = CachedExtension.Cached(
                (object key) =>
                {
                    queue.Enqueue(key);
                    ++i;
                    return i;
                });
            idMap(value);
            while (queue.Count > 0)
            {
                var o = queue.Dequeue();
                var type = o.GetType();
                var isType = type == typeof(Type);
                Base128.Serialize(isType ? idMap(type): 0, stream);
                if (isType)
                {
                    o = typeMap(type);
                }
                // serialize o.
                Serialize(idMap, stream, o);
            }
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
