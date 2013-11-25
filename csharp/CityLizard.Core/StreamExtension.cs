using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CityLizard
{
    public static class StreamExtension
    {
        public static bool IsEnd(this Stream stream)
        {
            return stream.Length <= stream.Position;
        }

        public static void Write(this Stream stream, byte[] byteArray)
        {
            stream.Write(byteArray, 0, byteArray.Length);
        }

        public static async void Write(this Stream stream, byte[] byteArray)
        {
            await stream.WriteAsync(byteArray, 0, byteArray.Length);
        }

        public static byte[] ReadByteArray(this Stream stream, int length)
        {
            var result = new byte[length];
            stream.Read(result, 0, length);
            return result;
        }

        public static void Write(this Stream stream, double value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static double ReadDouble(this Stream stream)
        {
            return BitConverter.ToDouble(stream.ReadByteArray(8), 0);
        }

        public static void Write(this Stream stream, float value)
        {
            stream.Write(BitConverter.GetBytes(value));
        }

        public static float ReadSingle(this Stream stream)
        {
            return BitConverter.ToSingle(stream.ReadByteArray(4), 0);
        }
    }
}
