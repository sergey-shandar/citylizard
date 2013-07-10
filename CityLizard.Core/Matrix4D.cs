using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard
{
    public struct Matrix4D<T>: IMatrix4D<T>
        where T: struct, IComparable<T>
    {
        public Vector4D<T> M0;
        public Vector4D<T> M1;
        public Vector4D<T> M2;
        public Vector4D<T> M3;

        public Matrix4D(
            Vector4D<T> m0, Vector4D<T> m1, Vector4D<T> m2, Vector4D<T> m3)
        {
            this.M0 = m0;
            this.M1 = m1;
            this.M2 = m2;
            this.M3 = m3;
        }

        public Matrix4D<T> Matrix { get { return this; } }
    }
}
