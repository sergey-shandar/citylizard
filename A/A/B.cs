using System;

namespace A
{
    public class B<T>
    {
        public abstract class C: System.IEquatable<C>
        {
            public abstract bool Equals(C other);
        }
    }
}
