using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf
{
    public sealed class ULongReader: ReadDelegate
    {
        private readonly Action<ulong> SetDelegate;

        public ULongReader(ILog log, Action<ulong> setDelegate): base(log)
        {
            SetDelegate = setDelegate;
        }

        override void Read(ulong value)
        {
            SetDelegate(value);
        }
    }

}
