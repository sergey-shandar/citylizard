using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard
{
    public interface IMatrix4D<I, T>
        where T: struct, IComparable<T>
        where I: struct, Policy.INumeric<T>
    {
        Matrix4D<I, T> Matrix { get; }
    }
}
