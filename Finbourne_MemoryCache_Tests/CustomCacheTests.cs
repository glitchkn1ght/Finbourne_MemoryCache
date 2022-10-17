using Finbourne_MemoryCache.Cache;
using Finbourne_MemoryCache.Config;
using Finbourne_MemoryCache.Interfaces;
using Finbourne_MemoryCache.Models;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;

namespace Finbourne_MemoryCache_Tests
{
    public class CustomCacheTests
    {
        CustomCache cache;

        [SetUp]
        public void Setup()
        {
            this.cache = new CustomCache();

        }

        [Test]
        public void WhenItemInCacheIsRetrieved_ThenLastAccessTimeIsUpdated()
        {
            DateTime timeAdded = this.cache.TryAddItemToCache("someKey", "someObject").CacheItem.LastTimeOfAccess;

            Thread.Sleep(2000);

            CacheItemResult retrievalResult = this.cache.TryGetItemFromCache("someKey");

            Assert.AreEqual(0, retrievalResult.StatusResult.StatusCode);

            Assert.AreNotEqual(timeAdded, retrievalResult.CacheItem.LastTimeOfAccess);
        }


    }
}