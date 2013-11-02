using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    interface IReadDelegate
    {
        void Variant(int field, ulong value);
        void Fixed64(int field, double value);
        void ByteArray(int field, byte[] value);
        void Fixed32(int field, float value);
    }

    enum WireType
    {
        VARIANT = 0,
        FIXED64 = 1,
        BYTE_ARRAY = 2,
        FIXED32 = 5,
    }

    sealed class ReadStream
    {
        public static void Read(Stream stream, IReadDelegate readDelegate)
        {
            while (stream.Position < stream.Length)
            {
                var header = (byte)stream.ReadByte();
                var field = header >> 3;
                var type = (WireType)(header & 0x07);
                switch(type)
                {
                    case WireType.VARIANT:
                        readDelegate.
                            Variant(field, Base128.Deserialize(stream));
                        break;
                    case WireType.FIXED64:
                        readDelegate.Fixed64(field, stream.ReadDouble());
                        break;
                    case WireType.BYTE_ARRAY:
                        readDelegate.
                            ByteArray(
                                field,
                                stream.ReadByteArray(
                                    (int)Base128.Deserialize(stream)
                                )
                            );
                        break;
                    case WireType.FIXED32:
                        readDelegate.Fixed32(field, stream.ReadSingle());
                        break;
                }
            }
        }
    }
}
