using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityLizard.Collections
{
    class PairFactory<K, T>
    {
        public readonly Func<K, T> Create;
        public readonly Action<K, T> Init;

        public PairFactory(Func<K, T> create, Action<K, T> init = null)
        {
            Create = create;
            Init = init ?? ((k, t) => {});
        }
    }
}
