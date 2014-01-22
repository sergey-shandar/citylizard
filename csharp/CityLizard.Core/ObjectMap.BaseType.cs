using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.ObjectMap
{
    abstract class BaseType
    {
        public readonly TypeCategory Category;

        public BaseType(Action<BaseType> register, TypeCategory category)
        {
            register(this);
            Category = category;
        }
    }
}
