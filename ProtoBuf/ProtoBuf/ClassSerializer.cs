using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    class ClassSerializer<T>: ISerializer<T>
        where T: new()
    {
        public ClassSerializer()
        {
            var type = typeof(T);
            foreach (var field in type.GetFields().Where(f => !f.IsStatic))
            {
                var fieldType = field.FieldType;
            }
        }

        public void Serialize(T value, Stream stream)
        {
        }

        public T Deserialize(Stream stream)
        {
            var value = new T();
            return value;
        }
    }
}
