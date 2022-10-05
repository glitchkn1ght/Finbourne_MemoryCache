using Finbourne_MemoryCache.Interfaces;
using Finbourne_MemoryCache.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Finbourne_MemoryCache.Cache
{
    public class CustomCache : ICustomCache
    {
        private ConcurrentDictionary<string, CacheItem> Cache { get; set; }

        public CustomCache()
        {
            this.Cache = new ConcurrentDictionary<string, CacheItem>();
        }

        public int GetCacheCount()
        {
            return this.Cache.Count;
        }

        public CacheItemResult TryAddItemToCache(string itemKey, object objectToStore)
        {
            CacheItemResult cacheItemResult = new CacheItemResult(objectToStore);

            cacheItemResult.CacheItem.LastTimeOfAccess = DateTime.UtcNow;

            bool additionSuccessful = this.Cache.TryAdd(itemKey, cacheItemResult.CacheItem);

            if (additionSuccessful)
            {
                cacheItemResult.StatusResult.StatusMessage += $"Item with key {itemKey} was successfully added to the cache. \n";
            }
            else
            {
                cacheItemResult.StatusResult.StatusCode = -105;
                cacheItemResult.StatusResult.StatusMessage += $"Item with Key {itemKey} is already present in dictionary, cannot add Item with duplicate key.\n";
            }

            return cacheItemResult;
        }

        public CacheItemResult EvictOldestItemFromCache(CacheItemResult cacheItemResult)
        {
            var item = this.Cache.FirstOrDefault(x => x.Value.LastTimeOfAccess == Cache.Values.Min(y => y.LastTimeOfAccess));

            CacheItem removedItem;
            bool removalResult = this.Cache.TryRemove(item.Key, out removedItem);

            if (!removalResult)
            {
                cacheItemResult.StatusResult.StatusCode = -104;
                cacheItemResult.StatusResult.StatusMessage += $"Cache is full but could not evict least recently used item with Key {item.Key} and LastAccessed {item.Value.LastTimeOfAccess} from the cache \n";
            }
            else
            {
                cacheItemResult.StatusResult.StatusMessage += $"Cache is full, the last recently used item with Key {item.Key} and LastAccessed {item.Value.LastTimeOfAccess} has been evicted from the cache \n";
            }

            return cacheItemResult;
        }

        public CacheItemResult TryGetItemFromCache(string itemKey)
        {
            CacheItemResult cacheItemResult = new CacheItemResult();
            CacheItem item;

            if (this.Cache.ContainsKey(itemKey))
            {
                if (this.Cache.TryGetValue(itemKey, out item))
                {
                    this.Cache[itemKey].LastTimeOfAccess = DateTime.UtcNow;

                    cacheItemResult.CacheItem = item;

                    cacheItemResult.StatusResult.StatusMessage = $"Item with Key {itemKey} was successfully retrieved from the cache. \n";
                }
                else
                {
                    cacheItemResult.StatusResult.StatusCode = -102;
                    cacheItemResult.StatusResult.StatusMessage = $"Item with Key {itemKey} was present in cache but could not be retrieved. \n";
                }
            }

            else
            {
                cacheItemResult.StatusResult.StatusCode = -103;
                cacheItemResult.StatusResult.StatusMessage = $"Item with Key {itemKey} was not present in the cache. \n";
            }

            return cacheItemResult;
        }
    }
}
