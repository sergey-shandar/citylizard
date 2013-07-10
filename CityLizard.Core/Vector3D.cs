using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard
{
    public struct Vector3D<T> where T: struct, IComparable<T>
    {
        public T X;
        public T Y;
        public T Z;

        public Vector3D(T x, T y, T z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
