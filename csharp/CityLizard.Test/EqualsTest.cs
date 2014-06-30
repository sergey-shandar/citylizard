using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityLizard.Test
{
    [TestClass]
    public class EqualsTest
    {
        public class A : IEquatable<A>
        {
            public bool Equals(A other)
            {
                return true;
            }
        }

        [TestMethod]
        public void TestMethod()
        {
            new A().Equals(5);
        }
    }
}
