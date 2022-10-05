using Finbourne_MemoryCache.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using Finbourne_MemoryCache.Interfaces;

namespace Finbourne_MemoryCache.CustomCache
{
    public sealed class CustomMemoryCache
    {
        private static volatile CustomMemoryCache instance;
        private static object syncRoot = new Object();
        private static int CacheSize { get; set; }

        private static ICacheOrchestrator CacheOrchestrator;

        private static ConcurrentDictionary<string, CacheItem> Cache { get; set; }
        private CustomMemoryCache() { }

        public static CustomMemoryCache GetInstance(int cacheSize, ICacheOrchestrator cacheOrchestrator)
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CustomMemoryCache();

                        CacheOrchestrator = cacheOrchestrator;
                        Cache = new ConcurrentDictionary<string, CacheItem>();
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
                    return cacheItemResult;
                }

                cacheItemResult = CacheOrchestrator.TryAddItemToCache(itemKey, objectToStore, Cache);
               
               if (cacheItemResult.StatusResult.StatusCode != 0)
               {
                    return cacheItemResult;
               }

               if (Cache.Count >= CacheSize)
               {
                   cacheItemResult = CacheOrchestrator.EvictOldestItemFromCache(cacheItemResult, Cache);
               }

                return cacheItemResult;
            }

            catch (Exception ex)
            {
                cacheItemResult.StatusResult.StatusCode = -111;
                cacheItemResult.StatusResult.ExceptionMessage = ex.Message;
                cacheItemResult.StatusResult.StatusMessage = "An exception occurred whilst updating the cache.";

                return cacheItemResult;
            }

        }

        public CacheItemResult GetFromCache(string itemKey)
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

                cacheItemResult = CacheOrchestrator.TryGetItemFromCache(itemKey, Cache);

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