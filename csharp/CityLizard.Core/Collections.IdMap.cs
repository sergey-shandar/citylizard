using System;
using System.Collections.Generic;
using CityLizard.Xml;

namespace CityLizard.Collections
{
    static class IdMap
    {
        public static CachedMap<K, ulong> Create<K>(Action<K, ulong> init)
        {
            ulong i = 0;
            return new CachedMap<K, ulong>(
                (k, register) => 
                {
                    ++i;
                    register(i);
                    init(k, i);
                });
        }
    }
}
