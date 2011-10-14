namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;
    using C = System.Collections.Generic;

    using P = Policy.Base;

    [N.TestFixture]
    public static class PolicyTest
    {
        public static T Sum<X, T>(this X x, params T[] e) 
            where X : struct, Policy.INumeric<T>
            where T: System.IComparable<T>
        {
            var r = x._0;
            foreach (var i in e)
            {
                r = x.Add(r, i);
            }
            return r;
        }

        [N.Test]
        public static void SumTest()
        {
            var r = P.X.Sum(3, 4, 5);
            var rl = P.X.Sum(3L, 4, 5);
        }
    }
}
