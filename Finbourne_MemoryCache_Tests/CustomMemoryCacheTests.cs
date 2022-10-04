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
        public void WhenItemRetrievedSuccessfully_ThenReturnSuccess()
        {
            CacheItemResult actual = this.CustomMemoryCache.GetItemFromCache("key1", "value1");

            Assert.AreEqual(0, actual.StatusResult.StatusCode);
        }
    }
}