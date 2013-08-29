using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Graphics
{
    public struct CompactMatrix4D<I, T>: IMatrix4D<T>
        where I: struct, Policy.INumeric<T>
        where T: struct, IComparable<T>
    {
        public Vector4D<T> X;
        public Vector4D<T> Y;
        public Vector4D<T> Z;

        public static readonly Vector4D<T> W = new Vector4D<T>(
            new I()._0, new I()._0, new I()._0, new I()._1);  

        public CompactMatrix4D(Vector4D<T> x, Vector4D<T> y, Vector4D<T> z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Matrix4D<T> Matrix
        {
            get { return new Matrix4D<T>(X, Y, Z, W); }
        }
    }
}
