using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CityLizard.ProtoBuf
{
    public interface ISerializer<T>
    {
        void Serialize(T value, Stream stream);
        T Deserialize(Stream stream);
    }
}
