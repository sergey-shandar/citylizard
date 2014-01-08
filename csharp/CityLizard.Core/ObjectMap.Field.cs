using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Core.ObjectMap
{
    sealed class Field
    {
        public readonly String Name;
        public readonly BaseType Type;

        public Field(String name, BaseType type)
        {
            Name = name;
            Type = type;
        }
    }
}
