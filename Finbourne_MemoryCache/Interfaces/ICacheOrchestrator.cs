using Finbourne_MemoryCache.Models;
using System.Collections.Concurrent;

namespace Finbourne_MemoryCache.Interfaces
{
    public interface ICacheOrchestrator
    {
        public CacheItemResult TryAddItemToCache(string itemKey, object objectToStore, ConcurrentDictionary<string, CacheItem> cache);

        public CacheItemResult EvictOldestItemFromCache(CacheItemResult cacheItemResult, ConcurrentDictionary<string, CacheItem> Cache);

        public CacheItemResult TryGetItemFromCache(string itemKey, ConcurrentDictionary<string, CacheItem> Cache);
    }
}
