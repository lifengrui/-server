using Server.cache.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.cache
{
    public class CacheFactory
    {
        public readonly static IAccountCache accountCache;

        static CacheFactory()
        {
            accountCache = new AccountCache();
        }
    }
}
