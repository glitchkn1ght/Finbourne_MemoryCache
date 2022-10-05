using Finbourne_MemoryCache.CustomCache;
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
        private readonly CustomMemoryCache CustomMemoryCache;
        private readonly CacheSettings CacheSettings;

        public CacheClient(ILogger<CacheClient> logger, IOptions<CacheSettings> cacheSettings)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.CacheSettings = cacheSettings.Value;

            this.CustomMemoryCache = CustomMemoryCache.GetInstance(this.CacheSettings.CacheSize, new CacheOrchestrator());
        }

        public void UseCache()
        {
            //I know this code is clunky but I wrote this class purely to demonstrate possible usage of the cache, not as an intended part of it's functionality. 
            //Thread sleep is to make times a bit easier to disnguish.

            this.GetCacheResult(this.CustomMemoryCache.AddToCache("SomeKey1", "SomeValue1"));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CustomMemoryCache.AddToCache("SomeKey2", 1));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CustomMemoryCache.AddToCache("SomeKey3", new Student(1001, "Bobby", "Baratheon")));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CustomMemoryCache.AddToCache("SomeKey4", "SomeValue4"));

            this.GetCacheResult(this.CustomMemoryCache.GetFromCache("SomeKey4"));
            this.GetCacheResult(this.CustomMemoryCache.GetFromCache("someInvalidKey"));
    
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
