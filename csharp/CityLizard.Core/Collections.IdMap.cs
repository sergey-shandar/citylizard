using System;
using System.Collections.Generic;
using CityLizard.Xml;

namespace CityLizard.Collections
{
    static class IdMap
    {
        public static CachedMap<K, ulong> Create<K>(Action<K, ulong> init = null)
        {
            ulong i = 0;
            return new CachedMap<K, ulong>(
                o => 
                {
                    ++i;
                    return i;
                },
                init);
        }
    }
}
