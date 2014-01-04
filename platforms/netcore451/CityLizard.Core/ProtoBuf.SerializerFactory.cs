using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using CityLizard.Collections.Extension;

namespace CityLizard.ProtoBuf
{
    public sealed class SerializerFactory
    {
        private readonly Dictionary<Type, object> Map =
            new Dictionary<Type, object>();

        private abstract class Field : ReadDelegate
        {
            private readonly FieldInfo Info;

            protected void ReadField(Object parent, Object value)
            {
                Info.SetValue(parent, value);
            }

            public void WriteField(Object parent, Stream stream)
            {
                Write(Info.GetValue(parent), stream);
            }

            protected abstract void Write(Object value, Stream stream); 

            protected Field(ILog log, FieldInfo info) : base(log) 
            {
                Info = info;
            }
        }

        private sealed class Serializer<T>: ISerializer<T>
        {
            private readonly Field[] FieldList;

            public void Serialize(T value, Stream stream)
            {
            }

            public T Deserialize(Stream stream)
            {
                return default(T);
            }

            public Serializer(SerializerFactory factory)
            {
                var type = typeof(T);
                factory.Map.Add(type, this);
                FieldList =
                    type.
                        GetTypeInfo().
                        DeclaredFields.
                        Where(f => !f.IsStatic).
                        Select((i, f) => new Field());
            }
        }

        private readonly ILog Log;

        public SerializerFactory(ILog log)
        {
            Log = log;
        }

        public ISerializer<T> Create<T>()
        {
            var type = typeof(T);
            var optional = Map.TryGet(type);
            return
                optional == null ?
                    new Serializer<T>(this):
                    (ISerializer<T>)optional.Value;
        }
    }
}
