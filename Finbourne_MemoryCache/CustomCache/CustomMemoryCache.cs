using Finbourne_MemoryCache.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finbourne_MemoryCache.CustomCache
{
    public sealed class CustomMemoryCache
    {
        private static volatile CustomMemoryCache instance;
        private static object syncRoot = new Object();
        private static int CacheSize { get; set; }

        private static Dictionary<string, CacheItem> Cache { get; set; }

        private CustomMemoryCache() { }

        public static CustomMemoryCache GetInstance(int cacheSize)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CustomMemoryCache();

                        Cache = new Dictionary<string, CacheItem>();
                        CacheSize = cacheSize;
                    }
                }
            }
            return instance;
        }

        public CacheItemResult AddToCache(string itemKey, object objectToStore)
        {
            CacheItemResult cacheItemResult = new CacheItemResult(objectToStore);
            try
            {                
                if (string.IsNullOrWhiteSpace(itemKey) || objectToStore == null)
                {
                    cacheItemResult.StatusResult.StatusCode = -101;
                    cacheItemResult.StatusResult.StatusMessage = $"Parameter error: Please check supplied parameters.";
                }

                if (Cache.Count >= CacheSize)
                {
                    cacheItemResult = EvictOldestItemFromCache(cacheItemResult);
                }

                if (cacheItemResult.StatusResult.StatusCode == 0)
                {
                    cacheItemResult.CacheItem.LastTimeOfAccess = DateTime.UtcNow;  
                    Cache.Add(itemKey, cacheItemResult.CacheItem);
                    cacheItemResult.StatusResult.StatusMessage += $"Item with key {itemKey} was successfully added to the cache";
                }

                return cacheItemResult;
            }

            catch (Exception ex)
            {
                cacheItemResult.StatusResult.StatusCode = -111;
                cacheItemResult.StatusResult.ExceptionMessage = ex.Message;
                cacheItemResult.StatusResult.StatusMessage = "An exception occurred whilst adding an item to the cache.";

                return cacheItemResult;
            }

        }

        private CacheItemResult EvictOldestItemFromCache(CacheItemResult cacheItemResult)
        {
            try
            {
                var item = Cache.FirstOrDefault(x => x.Value.LastTimeOfAccess == Cache.Values.Min(y => y.LastTimeOfAccess));

                if(item.Value == null)
                {
                    cacheItemResult.StatusResult.StatusCode = -103;
                    cacheItemResult.StatusResult.StatusMessage = "Could not retrieve oldest item from the cache, aborting addition of new item. \n";
                    return cacheItemResult;
                }

                Cache.Remove(item.Key);
                cacheItemResult.StatusResult.StatusMessage += $"Cache is full, the last recently used item with Key {item.Key} and LastAccessed {item.Value.LastTimeOfAccess} has been evicted from the cache \n";

                return cacheItemResult;
            }

            catch (Exception ex)
            {
                cacheItemResult.StatusResult.StatusCode = -112;
                cacheItemResult.StatusResult.ExceptionMessage = ex.Message;
                cacheItemResult.StatusResult.StatusMessage = "An exception occurred whilst evicting the oldest item from the cache.";

                return cacheItemResult;
            }
        }

        public CacheItemResult GetItemFromCache(string itemKey)
        {
            CacheItemResult cacheItemResult = new CacheItemResult();
            try
            {
                if (string.IsNullOrWhiteSpace(itemKey))
                {
                    cacheItemResult.StatusResult.StatusCode = -101;
                    cacheItemResult.StatusResult.StatusMessage = $"Parameter Error: Key supplied is null, empty or only consists of whitespace characters";
                    return cacheItemResult;
                }

                if (Cache.ContainsKey(itemKey))
                {
                    Cache[itemKey].LastTimeOfAccess = DateTime.UtcNow;

                    cacheItemResult.CacheItem.ObjectToCache = Cache[itemKey];
                    cacheItemResult.StatusResult.StatusMessage = $"Item with key {itemKey} was successfully retrieved from cache";
                }
                else
                {
                    cacheItemResult.StatusResult.StatusCode = -102;
                    cacheItemResult.StatusResult.StatusMessage = $"Item with Key {itemKey} was not present in cache.";
                }

                return cacheItemResult;
            }

            catch (Exception ex)
            {
                cacheItemResult.StatusResult.StatusCode = -110;
                cacheItemResult.StatusResult.ExceptionMessage = ex.Message;
                cacheItemResult.StatusResult.StatusMessage = $"An exception occurred while retrieving item with key {itemKey} from the cache";

                return cacheItemResult;
            }
        }
    }
}