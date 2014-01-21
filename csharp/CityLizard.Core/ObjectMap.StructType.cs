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
 
        public StructType(String name, Field[] fieldList):
            base(TypeCategory.Struct)
        {
            Name = name;
            FieldList = fieldList;
        }
    }
}
