using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Framework.G1.Leb128;

namespace CityLizard.ProtoBuf
{
    public static class IO
    {
        public static void Read<T>(
            Stream stream,
            Object value,
            IMessageRead messageRead
        )
            where T: class
        {
            while (!stream.IsEnd())
            {
                var header = (byte)stream.ReadByte();
                var field = header >> 3;
                var readDelegate = messageRead[field];
                var type = (WireType)(header & 0x07);
                switch (type)
                {
                    case WireType.VARIANT:
                        readDelegate.Read(value, V1.Value.Decode(() => (byte)stream.ReadByte()));
                        break;
                    case WireType.FIXED64:
                        readDelegate.Read(value, stream.ReadDouble());
                        break;
                    case WireType.BYTE_ARRAY:
                        readDelegate.
                            Read(
                                value,
                                stream.ReadByteArray(
                                    (int)V1.Value.Decode(() => (byte)stream.ReadByte())
                                )
                            );
                        break;
                    case WireType.FIXED32:
                        readDelegate.Read(value, stream.ReadSingle());
                        break;
                    default:
                        messageRead.Log.UnknownWireType(type);
                        return;
                }
            }
        }

        private static void WriteHeader(
            Stream stream, int field, WireType wireType)
        {
            stream.WriteByte((byte)((field << 3) | (byte)wireType));
        }

        private static void Write(Stream stream, IEnumerable<byte> bytes)
        {
            foreach (var b in bytes)
            {
                stream.Write(b);
            }
        }

        public static void Write(Stream stream, int field, ulong value)
        {
            WriteHeader(stream, field, WireType.VARIANT);
            Write(stream, V1.Value.Encode(value));
        }

        public static void Write(Stream stream, int field, long value)
        {
            WriteHeader(stream, field, WireType.VARIANT);
            Write(stream, V1.Value.Encode(ZigZag.Code(value)));
        }

        public static void Write(Stream stream, int field, double value)
        {
            WriteHeader(stream, field, WireType.FIXED64);
            stream.Write(value);
        }

        public static void Write(Stream stream, int field, float value)
        {
            WriteHeader(stream, field, WireType.FIXED32);
            stream.Write(value);
        }

        public static void Write(Stream stream, int field, byte[] byteArray)
        {
            WriteHeader(stream, field, WireType.BYTE_ARRAY);
            stream.Write(byteArray);
        }
    }
}
