using Finbourne_MemoryCache.Models;

namespace Finbourne_MemoryCache.Interfaces
{
    public interface ICustomMemoryCache
    {
        public CacheItemResult AddToCache(string Key, object objectToStore);

        public CacheItemResult GetItemFromCache(string itemKey);
    }
}
