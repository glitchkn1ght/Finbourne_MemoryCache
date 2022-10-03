using Finbourne_MemoryCache.Interfaces;
using Finbourne_MemoryCache.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Finbourne_MemoryCache.CustomCache
{
    public sealed class CustomMemoryCache : ICustomMemoryCache
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

        public CacheItemResult AddToCache(string key, object objectToStore)
        {
            CacheItemResult cacheItemResult = new CacheItemResult(objectToStore);
            try
            {
                if (Cache.Count >= CacheSize)
                {
                    cacheItemResult = EvictOldestItemFromCache(cacheItemResult);
                }

                if (cacheItemResult.Error.ErrorCode == 0)
                {
                    Cache.Add(key, cacheItemResult.CacheItem);
                    cacheItemResult.ResultMessage += $"Item with key {key} was successfully added to the cache";
                }

                return cacheItemResult;
            }

            catch (Exception ex)
            {
                cacheItemResult.Error.ErrorCode = -111;
                cacheItemResult.Error.ExceptionMessage = ex.Message;
                cacheItemResult.Error.ErrorMessage = "An exception occurred whilst adding an item to the cache.";

                return cacheItemResult;
            }

        }

        private static CacheItemResult EvictOldestItemFromCache(CacheItemResult cacheItemResult)
        {
            try
            {
                var item = Cache.FirstOrDefault(x => x.Value.LastTimeOfAccess == Cache.Values.Min(y => y.LastTimeOfAccess));

                if(item.Value == null)
                {
                    cacheItemResult.Error.ErrorCode = -103;
                    cacheItemResult.Error.ErrorMessage = "Could not retrieve oldest item from the cache, aborting addition of new item.";
                    return cacheItemResult;
                }

                Cache.Remove(item.Key);
                cacheItemResult.ResultMessage += $"As the cache is full the last recently used item with Key {item.Key} and Last time of access {item.Value.LastTimeOfAccess} has been evicted from the cache \n";

                return cacheItemResult;
            }

            catch (Exception ex)
            {
                cacheItemResult.Error.ErrorCode = -112;
                cacheItemResult.Error.ExceptionMessage = ex.Message;
                cacheItemResult.Error.ErrorMessage = "An exception occurred whilst evicting the oldest item from the cache.";

                return cacheItemResult;
            }
        }

        public CacheItemResult GetItemFromCache(string itemKey)
        {
            CacheItemResult cacheItemResult = new CacheItemResult();
            try
            {
                if (Cache.ContainsKey(itemKey))
                {
                    Cache[itemKey].LastTimeOfAccess = DateTime.Now;

                    cacheItemResult.CacheItem.ObjectInCache = Cache[itemKey];
                }
                else
                {
                    cacheItemResult.Error.ErrorCode = -102;
                    cacheItemResult.Error.ErrorMessage = $"Item with Key {itemKey} was not present in cache.";
                }

                cacheItemResult.ResultMessage += "Item was successfully retrieved from cache";

                return cacheItemResult;
            }

            catch (Exception ex)
            {
                cacheItemResult.Error.ErrorCode = -110;
                cacheItemResult.Error.ExceptionMessage = ex.Message;
                cacheItemResult.Error.ErrorMessage = $"An exception occurred while retrieving item with key {itemKey} from the cache";

                return cacheItemResult;
            }
        }
    }
}