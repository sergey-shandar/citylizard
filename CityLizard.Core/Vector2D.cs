using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard
{
    public struct Vector2D<I, T>
        where T : struct, IComparable<T>
        where I: struct, Policy.INumeric<T>
    {
        public T X;
        public T Y;
    }
}
