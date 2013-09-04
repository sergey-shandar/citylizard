using CityLizard.Policy;
using System;

namespace CityLizard.Graphics
{
    public static class Matrix
    {
        public static Matrix4D<T> New<T>(
            Vector4D<T> x, Vector4D<T> y, Vector4D<T> z, Vector4D<T> w)
            where T: struct, IComparable<T>
        {
            return new Matrix4D<T>(x, y, z, w);
        }

        public static CompactMatrix4D<I, T> CompactMatrix<I, T>(
            this I i, Vector4D<T> x, Vector4D<T> y, Vector4D<T> z)
            where I: struct, INumeric<T>
            where T: struct, IComparable<T>
        {
            return new CompactMatrix4D<I, T>(x, y, z);
        }

    };
}
