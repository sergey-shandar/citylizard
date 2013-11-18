using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLizard.Serializer
{
    internal class Optional<T>
    {
        public readonly T Value;

        public Optional(T value)
        {
            Value = value;
        }
    }
}
