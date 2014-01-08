using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Core.ObjectMap
{
    public class FixedArrayType: BaseType
    {
        public readonly ulong Size;
        public readonly BaseType ElementType;

        public FixedArrayType(bool isRef, ulong size, BaseType elementType):
            base(
                isRef ? 
                    TypeCategory.FIXED_ARRAY_REF:
                    TypeCategory.FIXED_ARRAY_BIT)
        {
            Size = size;
            ElementType = elementType;
        }
    }
}
