using Finbourne_MemoryCache.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using Finbourne_MemoryCache.Interfaces;

namespace Finbourne_MemoryCache.CustomCache
{
    public class CacheOrchestrator : ICacheOrchestrator
    {
        public CacheItemResult TryAddItemToCache(string itemKey, object objectToStore, ConcurrentDictionary<string, CacheItem> cache)
        {
            CacheItemResult cacheItemResult = new CacheItemResult(objectToStore);

            cacheItemResult.CacheItem.LastTimeOfAccess = DateTime.UtcNow;

            bool additionSuccessful = cache.TryAdd(itemKey, cacheItemResult.CacheItem);

            if (additionSuccessful)
            {
                cacheItemResult.StatusResult.StatusMessage += $"Item with key {itemKey} was successfully added to the cache. \n";
            }
            else
            {
                cacheItemResult.StatusResult.StatusCode = -104;
                cacheItemResult.StatusResult.StatusMessage += $"Key {itemKey} is already present in dictionary, cannot add Item duplicate key.\n";
            }

            return cacheItemResult;
        }

        public CacheItemResult EvictOldestItemFromCache(CacheItemResult cacheItemResult, ConcurrentDictionary<string, CacheItem> Cache)
        {
            var item = Cache.FirstOrDefault(x => x.Value.LastTimeOfAccess == Cache.Values.Min(y => y.LastTimeOfAccess));

            CacheItem removedItem;
            bool removalResult = Cache.TryRemove(item.Key, out removedItem);

            if (!removalResult)
            {
                cacheItemResult.StatusResult.StatusCode = -104;
                cacheItemResult.StatusResult.StatusMessage += $"Cache is bull but could not evict least recently used item with Key {item.Key} and LastAccessed {item.Value.LastTimeOfAccess} from cache \n";
            }
            else
            {
                cacheItemResult.StatusResult.StatusMessage += $"Cache is full, the last recently used item with Key {item.Key} and LastAccessed {item.Value.LastTimeOfAccess} has been evicted from the cache \n";
            }

            return cacheItemResult;
        }

        public CacheItemResult TryGetItemFromCache(string itemKey, ConcurrentDictionary<string, CacheItem> Cache)
        {
            CacheItemResult cacheItemResult = new CacheItemResult();
            CacheItem item;

            if (Cache.ContainsKey(itemKey))
            {
                if (Cache.TryGetValue(itemKey, out item))
                {
                    Cache[itemKey].LastTimeOfAccess = DateTime.UtcNow;

                    cacheItemResult.CacheItem = item;

                    cacheItemResult.StatusResult.StatusMessage = $"Item with key {itemKey} was successfully retrieved from cache";
                }
                else
                {
                    cacheItemResult.StatusResult.StatusCode = -102;
                    cacheItemResult.StatusResult.StatusMessage = $"Item with Key {itemKey} was present in cache but could not be retrieved;";
                }
            }

            else
            {
                cacheItemResult.StatusResult.StatusCode = -103;
                cacheItemResult.StatusResult.StatusMessage = $"Item with Key {itemKey} was not present in cache.";
            }

            return cacheItemResult;
        }
    }


}
