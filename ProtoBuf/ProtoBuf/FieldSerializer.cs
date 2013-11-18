using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    interface IFieldSerializer<T>
    {
        void Serialize(T value, Stream stream);
        ReadDelegate Deserializer(T value);
    }

    /*
    class FieldSerializer<T>: ISerializer<T>
    {
        public void Serialize(T value, System.IO.Stream stream)
        {
        }

        public T Deserialize(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }
    }
     * */
}
