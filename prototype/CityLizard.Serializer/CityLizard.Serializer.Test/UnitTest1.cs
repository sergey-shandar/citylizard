using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CityLizard.Serializer.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var document = CityLizard.Serializer.ObjectMap.Serialize(5);
        }
    }
}
