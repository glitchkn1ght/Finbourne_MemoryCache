using System;

namespace Finbourne_MemoryCache.Models
{
    public class CacheItem
    {
        public object ObjectToCache { get; set; }

        public DateTime LastTimeOfAccess { get; set; }

        public CacheItem() { }

        public CacheItem(object objectToStore)
        {
            this.ObjectToCache = objectToStore;
        }
    }

}
