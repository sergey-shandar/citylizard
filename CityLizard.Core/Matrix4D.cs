using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard
{
    public struct Matrix4D<I, T>: IMatrix4D<I, T>
        where T: struct, IComparable<T>
        where I: struct, Policy.INumeric<T>
    {
        public Vector4D<I, T> M0;
        public Vector4D<I, T> M1;
        public Vector4D<I, T> M2;
        public Vector4D<I, T> M3;

        public Matrix4D(
            Vector4D<I, T> m0,
            Vector4D<I, T> m1,
            Vector4D<I, T> m2,
            Vector4D<I, T> m3)
        {
            this.M0 = m0;
            this.M1 = m1;
            this.M2 = m2;
            this.M3 = m3;
        }

        public Matrix4D<I, T> Matrix { get { return this; } }
    }
}
