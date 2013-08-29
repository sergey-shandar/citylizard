using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Graphics
{
    public struct Matrix4D<T>: IMatrix4D<T>
        where T: struct, IComparable<T>
    {
        public Vector4D<T> X;
        public Vector4D<T> Y;
        public Vector4D<T> Z;
        public Vector4D<T> W;

        public Matrix4D(
            Vector4D<T> x, Vector4D<T> y, Vector4D<T> z, Vector4D<T> w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Matrix4D<T> Matrix { get { return this; } }
    }
}
