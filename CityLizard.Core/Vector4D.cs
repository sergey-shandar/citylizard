using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard
{
    public struct Vector4D<I, T>
        where T: struct, IComparable<T>
        where I: struct, Policy.INumeric<T>
    {
        public T X;
        public T Y;
        public T Z;
        public T W;
        public Vector4D(T x, T y, T z, T w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }
    }
}
