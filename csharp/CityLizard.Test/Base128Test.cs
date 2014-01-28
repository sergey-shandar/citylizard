using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityLizard.Test
{
    /// <summary>
    /// Summary description for Base128Test
    /// </summary>
    [TestClass]
    public class Base128Test
    {
        enum X: byte
        {
            
        }

        [TestMethod]
        public void Test()
        {
            var stream = new MemoryStream();
            Base128.Serialize(100, stream);
            //var x = (ulong)(object)(byte) 1;
            var x = (byte) (object) (X) 4;
        }
    }
}
