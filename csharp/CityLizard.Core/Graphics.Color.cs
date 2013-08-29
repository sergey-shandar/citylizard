using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Graphics
{
    public struct Color<T>
        where T: struct
    {
        public T R;
        public T G;
        public T B;
        public T A;

        public Color(T r, T g, T b, T a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }
    }
}
