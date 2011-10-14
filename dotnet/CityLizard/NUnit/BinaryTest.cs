namespace CityLizard.NUnit
{
    using N = global::NUnit.Framework;
    using CityLizard.Binary;

    [N.TestFixture]
    public class BinaryTest
    {
        [N.Test]
        public static void BTest()
        {
            N.Assert.AreEqual((ulong)B._0001, 0x1);
            N.Assert.AreEqual((ulong)B._0001._0010, 0x12);
            N.Assert.AreEqual((ulong)B._0011._0001._0010, 0x312);
            N.Assert.AreEqual((ulong)B._0011._0001._0010._0100, 0x3124);
            N.Assert.AreEqual((ulong)B._0101._0011._0001._0010._0100, 0x53124);
            N.Assert.AreEqual((ulong)B._0101._0011._0001._0010._0100._0110, 0x531246);
            N.Assert.AreEqual((ulong)B._0111._0101._0011._0001._0010._0100._0110, 0x7531246);
            N.Assert.AreEqual((ulong)B._0111._0101._0011._0001._0010._0100._0110._1000, 0x75312468);
            N.Assert.AreEqual((ulong)B._1001._0111._0101._0011._0001._0010._0100._0110._1000, 0x975312468);
            N.Assert.AreEqual((ulong)B._1001._0111._0101._0011._0001._0010._0100._0110._1000._1010, 0x975312468A);
            N.Assert.AreEqual((ulong)B._1011._1001._0111._0101._0011._0001._0010._0100._0110._1000._1010, 0xB975312468A);
            N.Assert.AreEqual((ulong)B._1011._1001._0111._0101._0011._0001._0010._0100._0110._1000._1010._1100, 0xB975312468AC);
            N.Assert.AreEqual((ulong)B._1101._1011._1001._0111._0101._0011._0001._0010._0100._0110._1000._1010._1100, 0xDB975312468AC);
            N.Assert.AreEqual((ulong)B._1101._1011._1001._0111._0101._0011._0001._0010._0100._0110._1000._1010._1100._1110, 0xDB975312468ACE);
            N.Assert.AreEqual((ulong)B._1111._1101._1011._1001._0111._0101._0011._0001._0010._0100._0110._1000._1010._1100._1110, 0xFDB975312468ACE);
        }
    }
}
