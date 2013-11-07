using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf
{
    public interface ILog
    {
        void Warning(PBException exception);
        void Error(PBException exception);
    }
}
