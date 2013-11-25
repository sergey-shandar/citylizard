using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.IO;
using CityLizard;

namespace UnitTestLibrary
{
    [TestClass]
    public class TestProtoBuf
    {
        private static void TestBase128(ulong value, params byte[] array)
        {
            var stream = new MemoryStream();
            Base128.Serialize(value, stream);
            var result = stream.ToArray();
            CollectionAssert.AreEqual(array, result);
            stream.Seek(0, SeekOrigin.Begin);
            var valueResult = Base128.Deserialize(stream);
            Assert.AreEqual(value, valueResult);
        }

        [TestMethod]
        public void TestSerializer()
        {
            TestBase128(0, 0);
            TestBase128(127, 127);
            TestBase128(255, 255, 1);
        }
    }
}
