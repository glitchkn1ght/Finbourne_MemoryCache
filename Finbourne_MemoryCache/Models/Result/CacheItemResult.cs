namespace Finbourne_MemoryCache.Models
{
    public class CacheItemResult
    {
        public CacheItemResult()
        {
            this.CacheItem = new CacheItem();
            this.StatusResult = new StatusResult(); 
        }

        public CacheItemResult(object objectToStore)
        {
            this.CacheItem = new CacheItem(objectToStore);
            this.StatusResult = new StatusResult();
        }

        public CacheItem CacheItem { get; set; }

        public StatusResult StatusResult { get; set; }
    }
}
