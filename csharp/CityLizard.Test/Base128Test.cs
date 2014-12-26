using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Framework.G1.Leb128;
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
            V1.Value.Encode(100);
            //var x = (ulong)(object)(byte) 1;
            var x = (byte) (object) (X) 4;
        }
    }
}
