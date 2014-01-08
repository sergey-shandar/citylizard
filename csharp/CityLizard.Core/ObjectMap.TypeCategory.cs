using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Core.ObjectMap
{
    /// BaseType:
    /// - RefType { Type }
    /// - SimpleType { Int | Float }
    /// - RecordType { Name, Filed[] }
    /// - FixedArrayType { Size, Type }
    /// - (ref only type) ArrayType { Type }
    /// 
    /// Base Types current:
    /// 1. IntType { Size }
    /// 2. UIntType { Size }
    /// 3. FloatType { Size }
    /// 4. DecimalType { Size }
    /// 5. StructType { Name, Filed[] }
    /// 6. ClassType { Base, Name, Field[] }
    /// 7. ArrayType { Dimension, Type }
    /// 
    /// Base Types:
    /// 1. Number { NumberType { SInt, UInt, BinaryFloat, DecimalFloat }, Size }
    /// 2. Struct { Name, Field[] }
    /// 3. Class { Base, Struct }
    /// 4. Array { Dimension, Type }
    /// 
    /// BaseType (future version):
    /// - MultiDimensionalArray
    ///     - 0 dimensions - reference: NO! Because OOP. So a reference type is
    ///       a special case. Alternative is OOP with no inheritance. 
    ///       Only interfaces.
    /// - Structure type
    ///     - Fixed Array Type.
    ///     - Record Type.
    ///     - Simply Type.
    ///
    /// Ref type is a partial case of a multidimencial array when dimension is 0
    /// and no inheritance.
    ///
    /// <summary>
    /// type:
    /// - struct (0)
    ///     - simple  (0)
    ///         - integer (0)
    ///             - signed   (0)
    ///                 - size 2^X
    ///             - unsigned (1)
    ///                 - size 2^X
    ///         - float   (1)
    ///             - binary   (0)
    ///                 - size 2^X
    ///             - decimal  (1)
    ///                 - size 2^X
    ///     - complex (1)
    ///         - record      (0)
    ///             - record (0)
    ///               (0 0000): ptr on array of fields
    ///         - fixed array (1)
    ///             - (0 0000): size, type 
    /// - class  (1)
    ///     - array   (0)
    ///         - (00 0000): dimension, ptr on element type
    ///     - complex (1) ...
    /// </summary>
    enum TypeCategory: byte
    {
        REF_BIT = 0x80,
        //
        COMPLEX_BIT = 0x40,
        //
        FLOAT_BIT = 0x20,
        FIXED_ARRAY_BIT = 0x20,
        //
        UNSIGNED_BIT = 0x10,
        DECIMAL_BIT = 0x10,
        //
        SIZE_POWER_MASK = 0x0F,
        //
        SIZE_8   = 0,
        SIZE_16  = 1,
        SIZE_32  = 2,
        SIZE_64  = 3,
        SIZE_128 = 4,
        /// 8 * 2^0 = 8, sbyte
        INT_8 = SIZE_8,
        /// 8 * 2^1 = 16, short
        INT_16 = SIZE_16,
        /// 8 * 2^2 = 32, int
        INT_32 = SIZE_32,
        /// 8 * 2^3 = 64, long
        INT_64 = SIZE_64,
        /// byte, boolean
        UINT_8 = UNSIGNED_BIT | SIZE_8,
        /// ushort, char
        UINT_16 = UNSIGNED_BIT | SIZE_16,
        /// uint
        UINT_32 = UNSIGNED_BIT | SIZE_32,
        /// ulong
        UINT_64 = UNSIGNED_BIT | SIZE_64,
        /// float
        FLOAT_32 = FLOAT_BIT | SIZE_32,
        /// double
        FLOAT_64 = FLOAT_BIT | SIZE_64,
        /// decimal
        DECIMAL_128 = FLOAT_BIT | DECIMAL_BIT | SIZE_128,
        /// array or string
        ARRAY_REF = REF_BIT,
        /// 
        RECORD = COMPLEX_BIT,
        /// 
        RECORD_REF = REF_BIT | COMPLEX_BIT,
        ///
        FIXED_ARRAY = COMPLEX_BIT | FIXED_ARRAY_BIT,
        ///
        FIXED_ARRAY_REF = REF_BIT | COMPLEX_BIT | FIXED_ARRAY_BIT,
    }
}
