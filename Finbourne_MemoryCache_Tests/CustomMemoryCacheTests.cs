using NUnit.Framework;
using Finbourne_MemoryCache.BusinessLogic;
using Finbourne_MemoryCache.CustomCache;
using Finbourne_MemoryCache.Models.ExampleClass;
using Finbourne_MemoryCache.Models;

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

            Assert.AreEqual(0, actual.Error.ErrorCode);
        }
    }
}