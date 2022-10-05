using Finbourne_MemoryCache.Cache;
using Finbourne_MemoryCache.Models;
using NUnit.Framework;
using Moq;
using Finbourne_MemoryCache.Interfaces;
using System.Collections.Concurrent;

namespace Finbourne_MemoryCache_Tests
{
    public class GetTests
    {
        //CustomMemoryCache CustomMemoryCache;
        //Mock<ICacheOrchestrator> CacheOrchestratorMock;

        //[SetUp]
        //public void Setup()
        //{
        //    this.CacheOrchestratorMock = new Mock<ICacheOrchestrator>();
        //    this.CustomMemoryCache = CustomMemoryCache.GetInstance(3, this.CacheOrchestratorMock.Object);
        //}

        //[TearDown]
        //public void Teardown()
        //{
        //    this.CustomMemoryCache = null;
        //    this.CacheOrchestratorMock = null;
        //}

        //[Test]
        //public void WhenItemPresentInCache_ThenGetItemReturnsSuccess()
        //{
        //    CacheItemResult expected = new CacheItemResult() { CacheItem = new CacheItem() { ObjectToCache = "value1" } };
        //    this.CacheOrchestratorMock.Setup(x => x.TryGetItemFromCache(It.IsAny<string>(), It.IsAny<ConcurrentDictionary<string, CacheItem>>())).
        //        Returns(expected);

        //    this.CustomMemoryCache = CustomMemoryCache.GetInstance(3, this.CacheOrchestratorMock.Object);

        //    CacheItemResult actual = this.CustomMemoryCache.GetFromCache("key1");

        //    Assert.AreEqual(expected.StatusResult.StatusCode, actual.StatusResult.StatusCode);
        //    Assert.AreEqual(expected.CacheItem.ObjectToCache, actual.CacheItem.ObjectToCache);

        //    this.CustomMemoryCache = null;
        //}
    }
}