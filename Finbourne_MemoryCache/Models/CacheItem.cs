using System;

namespace Finbourne_MemoryCache.Models
{
    public class CacheItem
    {
        public object ObjectInCache { get; set; }

        public DateTime LastTimeOfAccess { get; set; }

        public CacheItem() { }

        public CacheItem(object objectToStore)
        {
            this.ObjectInCache = objectToStore;
            this.LastTimeOfAccess = DateTime.Now;
        }
    }

}
