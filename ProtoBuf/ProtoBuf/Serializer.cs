using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProtoBuf
{
    //  map of declared types:
    //      sealed clases and value types:
    //          - T1 => ISerializer<T1>
    //          - T2 => ISerializer<T2>
    //          - ...
    //      extendable classes and interfaces:
    //          - T3 => ISerializer<T3> { TypeMap typeMap; } 
    //                  
    //  map of defined types:
    //      - T4 => ISerializable<T4>
    //
    //  register in both maps all types.
    public sealed class Serializer:
        ISerializer<ulong>,
        ISerializer<long>,
        ISerializer<uint>,
        ISerializer<int>,
        ISerializer<ushort>,
        ISerializer<short>,
        ISerializer<byte>,
        ISerializer<sbyte>,
        ISerializer<bool>,
        ISerializer<string>
    {
        // signed

        static void SignedSerialize(long value, Stream stream)
        {
            Base128.Serialize(ZigZag.Code(value), stream);
        }

        static long SignedDeserialize(Stream stream)
        {
            return ZigZag.Decode(Base128.Deserialize(stream));
        }

        // ulong

        public void Serialize(ulong value, Stream stream)
        {
            Base128.Serialize(value, stream);
        }

        public ulong Deserialize(Stream stream)
        {
            return Base128.Deserialize(stream);
        }

        // long

        public void Serialize(long value, Stream stream)
        {
            SignedSerialize(value, stream);
        }

        long ISerializer<long>.Deserialize(Stream stream)
        {
            return SignedDeserialize(stream);
        }

        // uint

        public void Serialize(uint value, Stream stream)
        {
            Base128.Serialize(value, stream);
        }

        uint ISerializer<uint>.Deserialize(Stream stream)
        {
            return (uint)Base128.Deserialize(stream);
        }

        // int

        public void Serialize(int value, Stream stream)
        {
            SignedSerialize(value, stream);
        }

        int ISerializer<int>.Deserialize(Stream stream)
        {
            return (int)SignedDeserialize(stream);
        }

        // ushort

        public void Serialize(ushort value, Stream stream)
        {
            Base128.Serialize(value, stream);
        }

        ushort ISerializer<ushort>.Deserialize(Stream stream)
        {
            return (ushort)Base128.Deserialize(stream);
        }

        // short

        public void Serialize(short value, Stream stream)
        {
            SignedSerialize(value, stream);
        }

        short ISerializer<short>.Deserialize(Stream stream)
        {
            return (short)SignedDeserialize(stream);
        }

        // byte

        public void Serialize(byte value, Stream stream)
        {
            Base128.Serialize(value, stream);
        }

        byte ISerializer<byte>.Deserialize(Stream stream)
        {
            return (byte)Base128.Deserialize(stream);
        }

        // sbyte

        public void Serialize(sbyte value, Stream stream)
        {
            SignedSerialize(value, stream);
        }

        sbyte ISerializer<sbyte>.Deserialize(Stream stream)
        {
            return (sbyte)SignedDeserialize(stream);
        }

        // string

        public void Serialize(string value, Stream stream)
        {
            var array = Encoding.UTF8.GetBytes(value);
            Base128.Serialize((ulong)array.Length, stream);
            stream.Write(array, 0, array.Length);
        }

        string ISerializer<string>.Deserialize(Stream stream)
        {
            var length = (int)Base128.Deserialize(stream);
            var array = new byte[length];
            stream.Read(array, 0, length);
            return Encoding.UTF8.GetString(array);
        }

        // bool

        public void Serialize(bool value, Stream stream)
        {
            stream.WriteByte(value ? (byte)1 : (byte)0);
        }

        bool ISerializer<bool>.Deserialize(Stream stream)
        {
            return stream.ReadByte() != 0;
        }
    }
}
