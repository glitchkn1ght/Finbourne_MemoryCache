using System;
using System.Collections.Generic;
using System.Text;
using Finbourne_MemoryCache.Models;

namespace Finbourne_MemoryCache.Models
{
    public class CacheItemResult
    {
        public CacheItemResult()
        {
            this.CacheItem = new CacheItem();
        }

        public CacheItemResult(object objectToStore)
        {
            this.CacheItem = new CacheItem(objectToStore);
        }

        public CacheItem CacheItem { get; set; }

        public string EvictionMessage { get; set; }

        public string ErrorMessage { get; set; }

        public int ErrorCode { get; set; }
    }
}
