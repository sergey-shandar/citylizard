using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.ProtoBuf
{
    public interface ILog
    {
        void UnexpectedType<T>();
        void UnknownWireType(WireType wireType);
        void UnknownField(int field);
    }
}
