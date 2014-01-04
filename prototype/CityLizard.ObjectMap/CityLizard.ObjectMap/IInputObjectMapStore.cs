using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLizard.ObjectMap
{
    public interface IInputObjectMapStore
    {
        ulong Count { get; }
        Func<ulong, byte[]> this[ulong i] { get; }
    }
}
