using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Graphics
{
    public struct Vector2D<T> where T : struct, IComparable<T>
    {
        public T X;
        public T Y;

        public Vector2D(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
