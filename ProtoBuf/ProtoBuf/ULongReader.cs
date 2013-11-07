using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf
{
    public sealed class ULongReader: IReadDelegate
    {
        private readonly ILog Log;
        private readonly Action<ulong> SetDelegate;

        public ULongReader(ILog log, Action<ulong> setDelegate)
        {
            Log = log;
            SetDelegate = setDelegate;
        }

        public void Variant(ulong value)
        {
            SetDelegate(value);
        }

        public void Fixed64(double value)
        {
            Log.Warning(new TypeMismatch(WireType.VARIANT, WireType.FIXED64));
        }

        public void ByteArray(byte[] value)
        {
            Log.Warning(new TypeMismatch(WireType.VARIANT, WireType.FIXED64));
        }

        public void Fixed32(float value)
        {
            Log.Warning(new TypeMismatch(WireType.VARIANT, WireType.FIXED64));
        }
    }

}
