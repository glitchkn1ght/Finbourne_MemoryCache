using Finbourne_MemoryCache.Config;
using Finbourne_MemoryCache.Interfaces;
using Finbourne_MemoryCache.Models;
using Microsoft.Extensions.Options;
using System;

namespace Finbourne_MemoryCache.Cache
{
    public interface ICacheWrapper
    {
        public CacheItemResult AddToCache(string itemKey, object objectToStore);

        public CacheItemResult GetFromCache(string itemKey);
    }
    
    public sealed class CacheWrapper : ICacheWrapper
    {
        private readonly ICustomCache CustomCache;

        private readonly CacheSettings CacheSettings;

        public CacheWrapper(IOptionsMonitor<CacheSettings> cacheSettings, ICustomCache customCache)
        {
            this.CacheSettings = cacheSettings.CurrentValue;
            this.CustomCache = customCache ?? throw new ArgumentNullException(nameof(customCache));
        }

        public CacheItemResult AddToCache(string itemKey, object objectToStore)
        {
            CacheItemResult cacheItemResult = new CacheItemResult(objectToStore);
            try
            {                
                if (string.IsNullOrWhiteSpace(itemKey))
                {
                    cacheItemResult.StatusResult.StatusCode = -101;
                    cacheItemResult.StatusResult.StatusMessage = $"Parameter error: Key supplied is null, empty or only consists of whitespace characters. \n";
                    return cacheItemResult;
                }

                if (objectToStore == null)
                {
                    cacheItemResult.StatusResult.StatusCode = -101;
                    cacheItemResult.StatusResult.StatusMessage = $"Parameter error: Object supplied is null. \n";
                    return cacheItemResult;
                }

                cacheItemResult = this.CustomCache.TryAddItemToCache(itemKey, objectToStore);
               
               if (cacheItemResult.StatusResult.StatusCode != 0)
               {
                    return cacheItemResult;
               }

               if (this.CustomCache.GetCacheCount() > CacheSettings.CacheSize)
               {
                   cacheItemResult = this.CustomCache.EvictOldestItemFromCache(cacheItemResult);
               }

                return cacheItemResult;
            }

            catch (Exception ex)
            {
                cacheItemResult.StatusResult.StatusCode = -111;
                cacheItemResult.StatusResult.ExceptionMessage = ex.Message;
                cacheItemResult.StatusResult.StatusMessage = "An exception occurred whilst updating the cache. \n";

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
                    cacheItemResult.StatusResult.StatusMessage = $"Parameter Error: Key supplied is null, empty or only consists of whitespace characters \n";
                    return cacheItemResult;
                }

                cacheItemResult = this.CustomCache.TryGetItemFromCache(itemKey);

                return cacheItemResult;
            }

            catch (Exception ex)
            {
                cacheItemResult.StatusResult.StatusCode = -110;
                cacheItemResult.StatusResult.ExceptionMessage = ex.Message;
                cacheItemResult.StatusResult.StatusMessage = $"An exception occurred while retrieving item with key {itemKey} from the cache \n";

                return cacheItemResult;
            }
        }
    }
}