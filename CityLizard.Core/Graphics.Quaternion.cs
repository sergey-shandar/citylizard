using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Graphics
{
    public struct Quaternion<T>
        where T: struct, IComparable<T>
    {
        public T X;
        public T Y;
        public T Z;
        public T W;

        public Quaternion(T x, T y, T z, T w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }
    }
}
