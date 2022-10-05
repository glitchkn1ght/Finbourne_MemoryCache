using Finbourne_MemoryCache.CustomCache;
using Finbourne_MemoryCache.Models;
using NUnit.Framework;
using Moq;
using Finbourne_MemoryCache.Interfaces;
using System.Collections.Concurrent;

namespace Finbourne_MemoryCache_Tests
{
    public class Tests
    {
        CustomMemoryCache CustomMemoryCache;
        Mock<ICacheOrchestrator> CacheOrchestratorMock;

        [SetUp]
        public void Setup()
        {
            this.CacheOrchestratorMock = new Mock<ICacheOrchestrator>();
            CustomMemoryCache.GetInstance(3, this.CacheOrchestratorMock.Object);
        }

        [Test]
        public void WhenItemAddedSuccessfully_ThenaAddItemReturnSuccess()
        {
            CacheItemResult expected = new CacheItemResult();
            this.CacheOrchestratorMock.Setup(x => x.TryAddItemToCache(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<ConcurrentDictionary<string,CacheItem>>())).
                Returns(expected);

            CacheItemResult actual = this.CustomMemoryCache.AddToCache("key1", "value1");

            Assert.AreEqual(0, actual.StatusResult.StatusCode);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase(null)]
        public void WhenItemKeyNullOrWhiteSpace_ThenaAddItemReturnNonSuccess101(string testKey)
        {
            CacheItemResult actual = this.CustomMemoryCache.AddToCache(testKey, "value1");

            Assert.AreEqual(-101, actual.StatusResult.StatusCode);
        }

        [TestCase(null)]
        public void WhenItemObjectNull_ThenaAddItemReturnNonSuccess101(string testObject)
        {
            CacheItemResult actual = this.CustomMemoryCache.AddToCache("key1", testObject);

            Assert.AreEqual(-101, actual.StatusResult.StatusCode);
        }

        //[Test]
        //public void WhenItemPresentInCache_ThenGetItemReturnsSuccess()
        //{
        //    CacheItemResult addResult = this.CustomMemoryCache.TryAddItemToCache("key1", "value1");

        //    CacheItemResult retrievalResult = this.CustomMemoryCache.GetItemFromCache("key1");

        //    Assert.AreEqual(0, retrievalResult.StatusResult.StatusCode);
        //}

        //[Test]
        //public void WhenItemNotPresentInCache_ThenGetItemReturnsNonSuccess102()
        //{
        //    CacheItemResult retrievalResult = this.CustomMemoryCache.GetItemFromCache("key2");

        //    Assert.AreEqual(-102, retrievalResult.StatusResult.StatusCode);
        //}

        //[Test]
        //public void WhenKeyIsNullOrWhiteSpace_ThenGetItemReturnsNonSuccess101()
        //{
        //    CacheItemResult retrievalResult = this.CustomMemoryCache.GetItemFromCache(null);

        //    Assert.AreEqual(-101, retrievalResult.StatusResult.StatusCode);
        //}
    }
}