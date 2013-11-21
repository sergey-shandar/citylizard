using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace CityLizard.Serializer.Test
{
    [TestClass]
    public class UnitTest
    {
        class X { };

        [TestMethod]
        public void TestMethod()
        {
            // var document = CityLizard.Serializer.ObjectMap.Serialize(5);
            var x = new CityLizard.Serializer.SerializerFactory();
            var classSerializer = x.CreateClassSerializer<X>();
        }
    }
}
