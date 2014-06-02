using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityLizard.Collections.Test
{
    [TestClass]
    public class CachedTest
    {
        [TestMethod]
        public void CollectionsCachedTest()
        {
            int called = 0;
            var f = CachedExtension.Cached(
                () =>
                {
                    ++called;
                    return called;
                });
            Assert.AreEqual(called, 0);
            Assert.AreEqual(f(), 1);
            Assert.AreEqual(called, 1);
            Assert.AreEqual(f(), 1);
        }
    }
}
