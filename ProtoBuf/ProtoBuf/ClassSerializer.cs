using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    /*
    class ReadDelegate<T>: IReadDelegate
        where T: new()
    {
        private T _Value = new T();

        public T Value { get { return _Value; } }        
    
        public void Variant(int field, ulong value)
        {
 	        throw new NotImplementedException();
        }

        public void Fixed64(int field, double value)
        {
 	        throw new NotImplementedException();
        }

        public void ByteArray(int field, byte[] value)
        {
 	        throw new NotImplementedException();
        }

        public void Fixed32(int field, float value)
        {
 	        throw new NotImplementedException();
        }
    }
     * */

    class ClassSerializer<T>: ISerializer<T>
        where T: class, new()
    {
        private readonly Func<T, IReadDelegate>[] factoryList;

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
            var result = new T();
            var arrayList = factoryList.Select(f => f(result)).ToArray();
            var size = arrayList.Length;
            ReadStream.Read(
                stream,
                field =>
                    field < size ? arrayList[field]: new VoidReadDelegate());
            return result;
        }
    }
}
