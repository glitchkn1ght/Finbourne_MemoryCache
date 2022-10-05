using Finbourne_MemoryCache.CustomCache;
using Finbourne_MemoryCache.Models;
using NUnit.Framework;

namespace Finbourne_MemoryCache_Tests
{
    public class Tests
    {
        CustomMemoryCache CustomMemoryCache;

        [SetUp]
        public void Setup()
        {
            this.CustomMemoryCache = CustomMemoryCache.GetInstance(3);
        }

        [Test]
        public void WhenItemAddedSuccessfully_ThenReturnSuccess()
        {
            CacheItemResult actual = this.CustomMemoryCache.AddToCache("key1", "value1");

            Assert.AreEqual(0, actual.StatusResult.StatusCode);
        }

        [Test]
        public void WhenItemPresentInCache_ThenGetItemReturnsSuccess()
        {
            CacheItemResult addResult = this.CustomMemoryCache.AddToCache("key1", "value1");

            CacheItemResult retrievalResult = this.CustomMemoryCache.GetItemFromCache("key1");

            Assert.AreEqual(0, retrievalResult.StatusResult.StatusCode);
        }

        [Test]
        public void WhenItemNotPresentInCache_ThenGetItemReturnsNonSuccess102()
        {
            CacheItemResult retrievalResult = this.CustomMemoryCache.GetItemFromCache("key2");

            Assert.AreEqual(-102, retrievalResult.StatusResult.StatusCode);
        }

        [Test]
        public void WhenKeyIsNullOrWhiteSpace_ThenGetItemReturnsNonSuccess101()
        {
            CacheItemResult retrievalResult = this.CustomMemoryCache.GetItemFromCache(null);

            Assert.AreEqual(-101, retrievalResult.StatusResult.StatusCode);
        }
    }
}