using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProtoBuf;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        private static void SerializerTest<T>(
            ISerializer<T> serializer, T value, params byte[] expected)
        {
            var base128 = new Serializer();
            var stream = new MemoryStream();
            serializer.Serialize(value, stream);
            var array = stream.ToArray();
            CollectionAssert.AreEqual(array, expected);
            stream.Seek(0, SeekOrigin.Begin);
            var result = serializer.Deserialize(stream);
            Assert.AreEqual(value, result);
        }

        private void ZigZagCheck(long value, ulong zigZag)
        {
            var newZigZag = ZigZag.Code(value);
            Assert.AreEqual(newZigZag, zigZag);
            var newValue = ZigZag.Decode(newZigZag);
            Assert.AreEqual(newValue, value);
        }

        [TestMethod]
        public void TestSerializer()
        {
            var serializer = new Serializer();
            // ulong
            SerializerTest(serializer, 0ul, 0);
            SerializerTest(serializer, 1ul, 1);
            SerializerTest(serializer, 127ul, 127);
            SerializerTest(serializer, 128ul, 128, 1);
            SerializerTest(serializer, 255ul, 255, 1);
            // 11111111  11111111  11111111  11111111 
            // 11111111  11111111  11111111  11111111
            // 1 1111111 1 1111111 1 1111111 1 1111111
            // 1 1111111 1 1111111 1 1111111 1 1111111
            // 1 1111111 0 0000001
            SerializerTest(
                serializer,
                ulong.MaxValue,
                255,
                255,
                255,
                255,
                255,
                255,
                255,
                255,
                255,
                1);
            // long
            SerializerTest(serializer, 0L, 0);
            SerializerTest(serializer, 1L, 2);
            SerializerTest(serializer, -1L, 1);
            SerializerTest(serializer, -64L, 127);
            SerializerTest(serializer, 64L, 128, 1);
            SerializerTest(serializer, 255L, 254, 3);
            // 11111111  11111111  11111111  11111111 
            // 11111111  11111111  11111111  11111111
            // 1 1111111 1 1111111 1 1111111 1 1111111
            // 1 1111111 1 1111111 1 1111111 1 1111111
            // 1 1111111 0 0000001
            SerializerTest(
                serializer,
                long.MinValue,
                255,
                255,
                255,
                255,
                255,
                255,
                255,
                255,
                255,
                1);
            SerializerTest(
                serializer,
                long.MaxValue,
                254,
                255,
                255,
                255,
                255,
                255,
                255,
                255,
                255,
                1);
            // uint
            SerializerTest(serializer, 0u, 0);
            SerializerTest(serializer, 1u, 1);
            SerializerTest(serializer, 127u, 127);
            SerializerTest(serializer, 128u, 128, 1);
            SerializerTest(serializer, 255u, 255, 1);
            // 11111111  11111111  11111111  11111111 =>
            // 1 1111111 1 1111111 1 1111111 1 1111111 0 0001111
            SerializerTest(
                serializer,
                uint.MaxValue,
                255, 255, 255, 255, 15);
            // int
            SerializerTest(serializer, 0, 0);
            SerializerTest(serializer, 1, 2);
            SerializerTest(serializer, -1, 1);
            SerializerTest(serializer, -64, 127);
            SerializerTest(serializer, 64, 128, 1);
            SerializerTest(serializer, 255L, 254, 3);
            SerializerTest(
                serializer,
                int.MinValue,
                255, 255, 255, 255, 15);
            SerializerTest(
                serializer,
                int.MaxValue,
                254, 255, 255, 255, 15);
            // ushort
            SerializerTest(serializer, (ushort)0, 0);
            SerializerTest(serializer, (ushort)1, 1);
            SerializerTest(serializer, (ushort)127, 127);
            SerializerTest(serializer, (ushort)128, 128, 1);
            SerializerTest(serializer, (ushort)255, 255, 1);
            // 11111111, 11111111 => 1 1111111, 1 1111111, 0 0000011 
            SerializerTest(serializer, ushort.MaxValue, 255, 255, 3);
            // short
            SerializerTest(serializer, (short)0, 0);
            SerializerTest(serializer, (short)1, 2);
            SerializerTest(serializer, (short)-1, 1);
            SerializerTest(serializer, (short)-64, 127);
            SerializerTest(serializer, (short)64, 128, 1);
            SerializerTest(serializer, (short)255L, 254, 3);
            SerializerTest(serializer, short.MinValue, 255, 255, 3);
            SerializerTest(serializer, short.MaxValue, 254, 255, 3);
            // byte
            SerializerTest(serializer, (byte)0, 0);
            SerializerTest(serializer, (byte)1, 1);
            SerializerTest(serializer, (byte)127, 127);
            SerializerTest(serializer, (byte)128, 128, 1);
            SerializerTest(serializer, (byte)255, 255, 1);
            // sbyte
            SerializerTest(serializer, (sbyte)0, 0);
            SerializerTest(serializer, (sbyte)1, 2);
            SerializerTest(serializer, (sbyte)-1, 1);
            SerializerTest(serializer, (sbyte)-64, 127);
            SerializerTest(serializer, (sbyte)64, 128, 1);
            SerializerTest(serializer, (sbyte)127, 254, 1);
            SerializerTest(serializer, (sbyte)-128, 255, 1);
            // string
            SerializerTest(serializer, "", 0);
            SerializerTest(
                serializer,
                "testing",
                0x07, 0x74, 0x65, 0x73, 0x74, 0x69, 0x6E, 0x67);
            // bool
            SerializerTest(serializer, false, 0);
            SerializerTest(serializer, true, 1);
        }

        [TestMethod]
        public void TestZigZag()
        {
            ZigZagCheck(0, 0);
            ZigZagCheck(1, 2);
            ZigZagCheck(-1, 1);
            ZigZagCheck(long.MinValue, ulong.MaxValue);
            ZigZagCheck(long.MaxValue, ulong.MaxValue - 1);
        }
    }
}
