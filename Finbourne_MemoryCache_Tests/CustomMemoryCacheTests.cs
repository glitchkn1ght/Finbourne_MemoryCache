using Finbourne_MemoryCache.Cache;
using Finbourne_MemoryCache.Models;
using NUnit.Framework;
using Moq;
using Finbourne_MemoryCache.Interfaces;
using System.Collections.Concurrent;
using Finbourne_MemoryCache.Config;
using Microsoft.Extensions.Options;

namespace Finbourne_MemoryCache_Tests
{
    public class Tests
    {
        Mock<ICustomCache> CustomCache;
        CacheWrapper CacheWrapper;


        [SetUp]
        public void Setup()
        {
            this.CustomCache = new Mock<ICustomCache>();

            var OptionsMock = new Mock<IOptionsMonitor<CacheSettings>>();
            OptionsMock.Setup(o => o.CurrentValue).Returns(new CacheSettings() {CacheSize =3 });


            this.CacheWrapper = new CacheWrapper(OptionsMock.Object, this.CustomCache.Object);
        }


        [Test]
        public void WhenItemAddedSuccessfully_ThenaAddItemReturnSuccess()
        {
            CacheItemResult expected = new CacheItemResult() { CacheItem = new CacheItem() { ObjectToCache = "value1" } };

            this.CustomCache.Setup(x => x.TryAddItemToCache(It.IsAny<string>(), It.IsAny<object>())).Returns(expected);
           
            CacheItemResult actual = this.CacheWrapper.AddToCache("key1", "value1");

            Assert.AreEqual(0, actual.StatusResult.StatusCode);
        }

        //[TestCase("")]
        //[TestCase(" ")]
        //[TestCase("  ")]
        //[TestCase(null)]
        //public void WhenItemKeyNullOrWhiteSpace_ThenaAddItemReturnNonSuccess101(string testKey)
        //{
        //    this.CustomMemoryCache = CustomMemoryCache.GetInstance(3, this.CacheOrchestratorMock.Object);

        //    CacheItemResult actual = this.CustomMemoryCache.AddToCache(testKey, "value1");

        //    Assert.AreEqual(-101, actual.StatusResult.StatusCode);
        //}

        //[TestCase(null)]
        //public void WhenItemObjectNull_ThenaAddItemReturnNonSuccess101(string testObject)
        //{
        //    this.CustomMemoryCache = CustomMemoryCache.GetInstance(3, this.CacheOrchestratorMock.Object);

        //    CacheItemResult actual = this.CustomMemoryCache.AddToCache("key1", testObject);

        //    Assert.AreEqual(-101, actual.StatusResult.StatusCode);
        //}

        //[Test]
        //public void WhenKeyIsNullOrWhiteSpace_ThenGetItemReturnsNonSuccess101()
        //{
        //    this.CustomMemoryCache = CustomMemoryCache.GetInstance(3, this.CacheOrchestratorMock.Object);

        //    CacheItemResult retrievalResult = this.CustomMemoryCache.GetFromCache(null);

        //    Assert.AreEqual(-101, retrievalResult.StatusResult.StatusCode);
        //}


        //[Test]
        //public void WhenKeyNotPresentInCache_ThenGetItemReturnsNonSuccess105()
        //{
        //    CacheItemResult expected = new CacheItemResult() { StatusResult = new StatusResult { StatusCode = -105 } };

        //    this.CacheOrchestratorMock.Setup(x => x.TryGetItemFromCache("someNotPresentKey", It.IsAny<ConcurrentDictionary<string, CacheItem>>())).Returns(expected);

        //    this.CustomMemoryCache = CustomMemoryCache.GetInstance(3, this.CacheOrchestratorMock.Object);

        //    CacheItemResult actual = this.CustomMemoryCache.GetFromCache("someNotPresentKey");

        //    Assert.AreEqual(expected.StatusResult.StatusCode, actual.StatusResult.StatusCode);
        //}




    }
}