using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Graphics
{
    public interface IMatrix4D<T> where T: struct, IComparable<T>
    {
        Matrix4D<T> Matrix { get; }
    }
}
