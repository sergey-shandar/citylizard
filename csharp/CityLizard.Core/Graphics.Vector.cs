using System;

namespace CityLizard.Graphics
{
    public static class Vector
    {
        public static Vector2D<T> New<T>(T x, T y)
            where T : struct, IComparable<T>
        {
            return new Vector2D<T>(x, y);
        }

        public static Vector3D<T> New<T>(T x, T y, T z)
            where T: struct, IComparable<T>
        {
            return new Vector3D<T>(x, y, z);
        }

        public static Vector4D<T> New<T>(T x, T y, T z, T w)
            where T: struct, IComparable<T>
        {
            return new Vector4D<T>(x, y, z, w);
        }

    };
}
