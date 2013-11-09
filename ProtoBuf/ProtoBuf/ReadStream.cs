using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    public sealed class ReadDelegate
    {
        public readonly Action<ulong> Variant;
        public readonly Action<double> Fixed64;
        public readonly Action<byte[]> ByteArray;
        public readonly Action<float> Fixed32;

        private static Action<T> Prepare<T>(ILog log, Action<T> action)
        {
            return action != null ? action : v => log.InvalidType<T>();
        }

        public ReadDelegate(
            ILog log,
            Action<ulong> variant = null,
            Action<double> fixed64 = null,
            Action<byte[]> byteArray = null,
            Action<float> fixed32 = null)
        {
            Variant = Prepare(log, variant);
            Fixed64 = Prepare(log, fixed64);
            ByteArray = Prepare(log, byteArray);
            Fixed32 = Prepare(log, fixed32);
        }

        public ReadDelegate(ILog log, Action<long> longAction):
            this(log: log, variant: value => longAction(ZigZag.Decode(value)))
        {
        }

        public ReadDelegate(ILog log, Action<uint> uintAction):
            this(log: log, variant: value => uintAction((uint)value))
        {
        }

        public ReadDelegate(ILog log, Action<string> stringAction):
            this(
                log: log,
                byteArray: 
                    byteArray => 
                        stringAction(Encoding.UTF8.GetString(byteArray))
            )
        {
        }
    }

    public enum WireType
    {
        VARIANT = 0,
        FIXED64 = 1,
        BYTE_ARRAY = 2,
        FIXED32 = 5,
    }

    sealed class ReadStream
    {
        public static void Read(
            Stream stream, Func<int, ReadDelegate> fieldReadDelegate)
        {
            while (!stream.IsEnd())
            {
                var header = (byte)stream.ReadByte();
                var field = header >> 3;
                var readDelegate = fieldReadDelegate(field);
                var type = (WireType)(header & 0x07);
                switch(type)
                {
                    case WireType.VARIANT:
                        readDelegate.Variant(Base128.Deserialize(stream));
                        break;
                    case WireType.FIXED64:
                        readDelegate.Fixed64(stream.ReadDouble());
                        break;
                    case WireType.BYTE_ARRAY:
                        readDelegate.
                            ByteArray(
                                stream.ReadByteArray(
                                    (int)Base128.Deserialize(stream)
                                )
                            );
                        break;
                    case WireType.FIXED32:
                        readDelegate.Fixed32(stream.ReadSingle());
                        break;
                    default:
                        return;
                }
            }
        }

        private static void WriteHeader(
            Stream stream, int field, WireType wireType)
        {
            stream.WriteByte((byte)((field << 3) | (byte)wireType));
        }

        public static void Write(Stream stream, int field, ulong value)
        {
            WriteHeader(stream, field, WireType.VARIANT);
            Base128.Serialize(value, stream);
        }

        public static void Write(Stream stream, int field, double value)
        {
            WriteHeader(stream, field, WireType.FIXED64);
            stream.Write(value);
        }

        public static void Write(Stream stream, int field, byte[] byteArray)
        {
            WriteHeader(stream, field, WireType.BYTE_ARRAY);
            stream.Write(byteArray);
        }

        public static void Write(Stream stream, int field, float value)
        {
            WriteHeader(stream, field, WireType.FIXED64);
            stream.Write(value);
        }
    }
}
