using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CityLizard.Collections;

namespace CityLizard.ObjectMap
{
    sealed class Field
    {
        public readonly String Name;
        readonly CachedValue<BaseType> _Type;

        public BaseType Type { get { return _Type.Value; } }

        public Field(String name, Func<BaseType> type)
        {
            Name = name;
            _Type = type.CachedValue();
        }
    }
}
