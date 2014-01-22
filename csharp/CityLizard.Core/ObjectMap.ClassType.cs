using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CityLizard.Collections;

namespace CityLizard.ObjectMap
{
    sealed class ClassType: BaseType
    {
        public readonly String Name;

        readonly CachedValue<BaseType> _Base;

        public BaseType Base { get { return _Base.Value; } }

        public readonly Field[] FieldList;

        public ClassType(
            Action<BaseType> register,
            String name,
            Func<BaseType> _base,
            Field[] fieldList):
            base(register, TypeCategory.Class)
        {
            Name = name;
            _Base = _base.CachedValue();
            FieldList = fieldList;
        }
    }
}
