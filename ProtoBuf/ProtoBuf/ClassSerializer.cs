using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    class ClassSerializer<T>: ISerializer<T>
        where T: class, new()
    {
        private readonly ReadDelegate DefaultReadDelegate;
        private readonly IFieldSerializer<T>[] factoryList;

        public ClassSerializer(ILog log)
        {
            DefaultReadDelegate = new ReadDelegate(log);
            var type = typeof(T);
            foreach (var field in type.GetFields().Where(f => !f.IsStatic))
            {
                var fieldType = field.FieldType;
            }
        }

        public void Serialize(T value, Stream stream)
        {
            var size = factoryList.Length;
            for(var i = 0; i < size; ++i)
            {
                factoryList[i].Serialize(value, stream);
            }
        }

        public T Deserialize(Stream stream)
        {
            var result = new T();
            var arrayList =
                factoryList.Select(f => f.Deserializer(result)).ToArray();
            var size = arrayList.Length;
            ReadStream.Read(
                stream,
                field =>
                    field < size ? arrayList[field]: DefaultReadDelegate);
            return result;
        }
    }
}
