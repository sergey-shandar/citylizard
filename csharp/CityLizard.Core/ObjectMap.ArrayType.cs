using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CityLizard.Collections;

namespace CityLizard.ObjectMap
{
    sealed class ArrayType: BaseType
    {
        public readonly byte Dimension;
        public readonly CachedValue<BaseType> _ElementType;

        public BaseType ElementType
        {
            get { return _ElementType.Value; }
        }

        public ArrayType(byte dimentsion, Func<BaseType> elementType):
            base(TypeCategory.Array)
        {
            Dimension = dimentsion;
            _ElementType = elementType.CachedValue();
        }

        public override void Serialize(Stream serialize, object value)
        {
            throw new NotImplementedException();
        }
    }
}
