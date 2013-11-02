using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    public class ArraySerializer<T>: ISerializer<T[]>
    {
        private readonly ISerializer<T> ElementSerializer;

        public ArraySerializer(ISerializer<T> elementSerializer)
        {
            ElementSerializer = elementSerializer;
        }

        public void Serialize(T[] value, Stream stream)
        {
            var length = (ulong)value.LongLength;
            Base128.Serialize(length, stream);
            foreach(var element in value)
            {
                ElementSerializer.Serialize(element, stream);
            }
        }

        public T[] Deserialize(Stream stream)
        {
            var length = Base128.Deserialize(stream);
            var result = new T[length];
            for (var i = 0UL; i < length; ++i)
            {
                result[i] = ElementSerializer.Deserialize(stream);
            }
            return result;
        }
    }
}
