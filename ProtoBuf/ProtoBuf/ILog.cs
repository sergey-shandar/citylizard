using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf
{
    public interface ILog
    {
        void InvalidType<T>(T value);
    }
}
