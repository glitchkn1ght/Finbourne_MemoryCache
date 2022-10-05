using Finbourne_MemoryCache.Cache;
using Finbourne_MemoryCache.Models;
using NUnit.Framework;
using Moq;
using Finbourne_MemoryCache.Interfaces;
using System.Collections.Concurrent;
using Finbourne_MemoryCache.Config;
using Microsoft.Extensions.Options;
using System;

namespace Finbourne_MemoryCache_Tests
{
    public class CacheWrapperTests
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
            Assert.AreEqual("value1", actual.CacheItem.ObjectToCache);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase(null)]
        public void WhenItemKeyNullOrWhiteSpace_ThenaAddItemReturnNonSuccess101(string testKey)
        {
            CacheItemResult actual = this.CacheWrapper.AddToCache(testKey, "value1");

            Assert.AreEqual(-101, actual.StatusResult.StatusCode);
        }

        [TestCase(null)]
        public void WhenItemObjectNull_ThenaAddItemReturnNonSuccess101(string testObject)
        {
            CacheItemResult actual = this.CacheWrapper.AddToCache("key1", testObject);

            Assert.AreEqual(-101, actual.StatusResult.StatusCode);
        }

        [Test]
        public void WhenAddToCacheThrowsException_ThenAddItemReturnsNonSuccess110()
        {
            this.CustomCache.Setup(x => x.TryAddItemToCache("someKey", "someValue")).Throws(new Exception());

            CacheItemResult actual = this.CacheWrapper.AddToCache("someKey", "someValue");

            Assert.AreEqual(-111, actual.StatusResult.StatusCode);
        }


        [Test]
        public void WhenKeyIsNullOrWhiteSpace_ThenGetItemReturnsNonSuccess101()
        {
            CacheItemResult retrievalResult = this.CacheWrapper.GetFromCache(null);

            Assert.AreEqual(-101, retrievalResult.StatusResult.StatusCode);
        }


        [Test]
        public void WhenKeyNotPresentInCache_ThenGetItemReturnsNonSuccess105()
        {
            CacheItemResult expected = new CacheItemResult() { StatusResult = new StatusResult { StatusCode = -105 } };

            this.CustomCache.Setup(x => x.TryGetItemFromCache("someNotPresentKey")).Returns(expected);

            CacheItemResult actual = this.CacheWrapper.GetFromCache("someNotPresentKey");

            Assert.AreEqual(expected.StatusResult.StatusCode, actual.StatusResult.StatusCode);
        }

        [Test]
        public void WhenGetFromCacheThrowsException_ThenGetItemReturnsNonSuccess110()
        {
            this.CustomCache.Setup(x => x.TryGetItemFromCache("someNotPresentKey")).Throws(new Exception());

            CacheItemResult actual = this.CacheWrapper.GetFromCache("someNotPresentKey");

            Assert.AreEqual(-110, actual.StatusResult.StatusCode);
        }
    }
}