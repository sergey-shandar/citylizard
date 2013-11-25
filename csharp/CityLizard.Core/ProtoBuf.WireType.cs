using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.ProtoBuf
{
    public enum WireType
    {
        VARIANT = 0,
        FIXED64 = 1,
        BYTE_ARRAY = 2,
        FIXED32 = 5,
    }
}
