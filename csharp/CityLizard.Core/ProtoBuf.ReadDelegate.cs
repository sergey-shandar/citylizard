using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.ProtoBuf
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

        public ReadDelegate(ILog log, Action<long> longAction) :
            this(
                 log: log,
                 variant: value => longAction(ZigZag.Decode(value))
             )
        {
        }

        public ReadDelegate(ILog log, Action<uint> uintAction) :
            this(
                 log: log,
                 variant: value => uintAction((uint)value)
             )
        {
        }

        public ReadDelegate(ILog log, Action<int> intAction) :
            this(
                 log: log,
                 variant: value => intAction((int)ZigZag.Decode(value))
             )
        {
        }

        public ReadDelegate(ILog log, Action<ushort> ushortAction) :
            this(
                 log: log,
                 variant: value => ushortAction((ushort)value)
             )
        {
        }

        public ReadDelegate(ILog log, Action<short> shortAction) :
            this(
                 log: log,
                 variant: value => shortAction((short)ZigZag.Decode(value))
             )
        {
        }

        public ReadDelegate(ILog log, Action<byte> byteAction) :
            this(
                 log: log,
                 variant: value => byteAction((byte)value)
             )
        {
        }

        public ReadDelegate(ILog log, Action<sbyte> sbyteAction) :
            this(
                 log: log,
                 variant: value => sbyteAction((sbyte)ZigZag.Decode(value))
             )
        {
        }

        public ReadDelegate(ILog log, Action<string> stringAction) :
            this(
                 log: log,
                 byteArray:
                     byteArray => 
                         stringAction(Encoding.UTF8.GetString(byteArray))
             )
        {
        }
    }
}
