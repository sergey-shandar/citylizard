using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CityLizard.Core;

namespace CityLizard.Core.ObjectMap
{
    class BaseType
    {
        public readonly TypeCategory Category;

        protected BaseType(TypeCategory category)
        {
            Category = category;
        }
    }
}
