using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityLizard.Test
{
    [TestClass]
    public class ObjectMapTest
    {
        class X { }

        [TestMethod]
        public void Test()
        {
            var document = ObjectMap.Map.Serialize(new X());

            //
            var d = new Dictionary<object, Func<int>>();
        }
    }
}
