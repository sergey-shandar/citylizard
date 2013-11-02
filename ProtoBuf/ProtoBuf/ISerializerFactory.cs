using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf
{
    interface ISerializerFactory
    {
        ISerializer<T> CreateSerializer<T>();
    }
}
