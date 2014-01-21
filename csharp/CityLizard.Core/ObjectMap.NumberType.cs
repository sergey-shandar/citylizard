using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.ObjectMap
{
    public enum NumberCategory: byte
    {
        Int = 0,
        UInt = 1,
        Float = 2,
        Decimal = 3,
    }

    sealed class NumberType: BaseType
    {
        public readonly NumberCategory NumberCategory;

        public readonly byte SizeExp;

        /// <summary>
        /// Size = SizeExp ^ 2
        /// </summary>
        public ulong Size 
        {
            get { return 1UL << SizeExp; } 
        }

        public NumberType(NumberCategory numberCategory, byte sizeExp):
            base(TypeCategory.Number)
        {
            NumberCategory = numberCategory;
            SizeExp = sizeExp;
        }
    }
}
