using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityLizard.Test
{
    [TestClass]
    public class DictionaryTest
    {
        [TestMethod]
        public void Test()
        {
            var map = new Dictionary<object, int>();
            /*
            map.Add(null, 0);
            var i = map[null];
            Assert.AreEqual(0, i);
             * */
        }
    }
}
