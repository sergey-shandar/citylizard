using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityLizard.ObjectMap
{
    public interface IOutputObjectMapStore
    {
        IOutputObjectStore New();
    }
}
