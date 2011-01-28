namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;
    using C = System.Collections.Generic;

    using P = Policy.Base;

    [N.TestFixture]
    public static class PolicyTest
    {
        public static T Sum<P, T>(this P p, params T[] e) 
            where P : Policy.INumeric<T>
        {
            var r = p.Zero();
            foreach (var i in e)
            {
                r = p.Add(r, i);
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
