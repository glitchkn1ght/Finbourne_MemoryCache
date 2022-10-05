using Finbourne_MemoryCache.Models;
using System.Collections.Concurrent;

namespace Finbourne_MemoryCache.Interfaces
{
    public interface ICustomCache
    {
        public CacheItemResult TryAddItemToCache(string itemKey, object objectToStore);

        public CacheItemResult EvictOldestItemFromCache(CacheItemResult cacheItemResult);

        public CacheItemResult TryGetItemFromCache(string itemKey);

        public int GetCacheCount();
    }
}
