namespace CityLizard.Fsm
{
    using C = System.Collections.Generic;
    using S = System;

    using System.Linq;

    public class Name : C.HashSet<int>, S.IEquatable<Name>
    {
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Name);
        }

        public bool Equals(Name other)
        {
            return base.SetEquals(other);
        }

        /// <summary>
        /// Note: the function implementation has really bad distribution. 
        /// However it works.
        /// Requirements: { 0, 1 }.GetHachCode() == { 1, 0 }.GetHashCode().
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            uint hash = 0;
            foreach (var i in this)
            {
                hash ^= (uint)i;
            }
            return (int)hash;
        }
    }
}
