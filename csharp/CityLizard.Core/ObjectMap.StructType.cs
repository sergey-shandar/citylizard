using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.ObjectMap
{
    sealed class StructType: BaseType
    {
        public readonly String Name;
        public readonly Field[] FieldList;
 
        public StructType(
            Action<BaseType> register, String name, Field[] fieldList):
            base(register, TypeCategory.Struct)
        {
            Name = name;
            FieldList = fieldList;
        }
    }
}
