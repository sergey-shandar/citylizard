using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Core.ObjectMap
{
    sealed class ArrayType: BaseType
    {
        public readonly byte Dimension;
        public readonly BaseType ElementType;

        public ArrayType(byte dimentsion, BaseType elementType):
            base(TypeCategory.ARRAY_REF)
        {
            Dimension = dimentsion;
            ElementType = elementType;
        }
    }
}
