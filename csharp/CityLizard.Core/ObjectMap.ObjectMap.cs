using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CityLizard.Collections.Extension;

namespace CityLizard.Core.ObjectMap
{
    public sealed class ObjectMap
    {
        private readonly Dictionary<Object, ulong> Map =
            new Dictionary<object, ulong>();

        private ulong Serialize(Type type)
        {
            // TODO
            return 0;
        }

        public ulong Serialize(ulong type, Object value)
        {
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
                        return Serialize((Type)value);
                    }
                    else
                    {
                        return Serialize(Serialize(type), value);
                        // TODO
                    }
                }
            }
        }
    }
}
