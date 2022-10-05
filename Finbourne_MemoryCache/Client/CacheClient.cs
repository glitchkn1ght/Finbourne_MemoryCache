using Finbourne_MemoryCache.Cache;
using Finbourne_MemoryCache.Interfaces;
using Finbourne_MemoryCache.Models;
using Finbourne_MemoryCache.Models.Config;
using Finbourne_MemoryCache.Models.ExampleClass;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace Finbourne_MemoryCache.Client
{
    public class CacheClient
    {
        private readonly ILogger<CacheClient> Logger;
        private readonly ICacheWrapper CacheWrapper;

        public CacheClient(ILogger<CacheClient> logger, ICacheWrapper cacheWrapper)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.CacheWrapper = cacheWrapper ?? throw new ArgumentNullException(nameof(cacheWrapper));
        }

        public void UseCache()
        {
            //I know this code is clunky but I wrote this class purely to demonstrate possible usage of the cache, not as an intended part of it's functionality. 
            //Thread sleep is to make times a bit easier to disnguish.

            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey1", "SomeValue1"));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey2", 1));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey3", new Student(1001, "Bobby", "Baratheon")));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey4", "SomeValue4"));

            this.GetCacheResult(this.CacheWrapper.GetFromCache("SomeKey4"));
            this.GetCacheResult(this.CacheWrapper.GetFromCache("someInvalidKey"));
    
            Console.WriteLine("---END---");
            Console.ReadLine();
        }

        private void GetCacheResult(CacheItemResult result)
        {
            if (result.StatusResult.StatusCode == 0)
            {
                Console.WriteLine(result.StatusResult.StatusMessage);
                this.Logger.LogInformation($"Operation=GetCacheResult(CustomeCacheOrchestrator), Status=Success, Message={result.StatusResult.StatusMessage}");
            }
            else
            {
                Console.WriteLine($"An Error Occurred: Code={result.StatusResult.StatusCode}, Message={result.StatusResult.StatusMessage}");

                if (result.StatusResult.ExceptionMessage != null)
                {
                    this.Logger.LogError($"Operation=GetCacheResult(CustomeCacheOrchestrator), Status=Failure, Message={result.StatusResult.ExceptionMessage}");
                }
            }
        }
    }
}
