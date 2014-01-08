using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Core.ObjectMap
{
    sealed class RecordType: BaseType
    {
        public readonly String Name;
        public readonly Field[] FieldList;
 
        public RecordType(bool isRef, String name, Field[] fieldList):
            base(isRef ? TypeCategory.RECORD_REF: TypeCategory.RECORD)
        {
            Name = name;
            FieldList = fieldList;
        }
    }
}
