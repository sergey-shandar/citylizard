using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard
{
    public struct CompactMatrix4D<I, T>: IMatrix4D<I, T>
        where I: struct, Policy.INumeric<T>
        where T: struct, IComparable<T>
    {
        public Vector4D<I, T> M0;
        public Vector4D<I, T> M1;
        public Vector4D<I, T> M2;
        public Vector4D<I, T> M3
        {
            get
            {
                var i = new I();
                var _0 = i._0;
                return new Vector4D<I, T>(_0, _0, _0, i._1);
            }
        }

        public CompactMatrix4D(
            Vector4D<I, T> m0, Vector4D<I, T> m1, Vector4D<I, T> m2)
        {
            this.M0 = m0;
            this.M1 = m1;
            this.M2 = m2;
        }

        public Matrix4D<I, T> Matrix
        {
            get { return new Matrix4D<I, T>(M0, M1, M2, M3); }
        }
    }
}
