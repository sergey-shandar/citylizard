using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.ObjectMap
{
    /// <summary>
    /// Base Types:
    /// 1. Number { NumberType { SInt, UInt, BinaryFloat, DecimalFloat }, Size }
    /// 2. Struct { Name, Field[] }
    /// 3. Class { Base, Struct }
    /// 4. Array { Dimension, Type }
    /// </summary>
    enum TypeCategory: byte
    {
        Number = 0,
        Struct = 1,
        Array = 2,
        Class = 3,
    }

}
