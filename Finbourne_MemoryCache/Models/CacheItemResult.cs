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
            this.Error = new ErrorResult(); 
        }

        public CacheItemResult(object objectToStore)
        {
            this.CacheItem = new CacheItem(objectToStore);
            this.Error = new ErrorResult();
        }

        public CacheItem CacheItem { get; set; }

        public ErrorResult Error { get; set; }

        public string EvictionMessage { get; set; }
    }
}
