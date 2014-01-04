using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CityLizard.ObjectMap
{
    public interface IOutputObjectStore
    {
        ulong Id { get; }
        void Write(byte[] value);
    }
}
