using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoBuf
{
    public class PBException: Exception
    {
        public PBException(string message): base(message)
        {
        }
    }
}
