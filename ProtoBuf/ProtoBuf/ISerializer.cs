using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    public interface ISerializer<T>
    {
        void Serialize(T value, Stream stream);
        T Deserialize(Stream stream);
    }
}
