using Finbourne_MemoryCache.CustomCache;
using Finbourne_MemoryCache.Models;
using Finbourne_MemoryCache.Models.Config;
using Finbourne_MemoryCache.Models.ExampleClass;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace Finbourne_MemoryCache.BusinessLogic
{
    public class CustomeCache_Client
    {
        private readonly ILogger<CustomeCache_Client> Logger;
        private readonly CustomMemoryCache CustomMemoryCache;
        private readonly CacheSettings CacheSettings;

        public CustomeCache_Client(ILogger<CustomeCache_Client> logger, IOptions<CacheSettings> cacheSettings)
        {
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.CacheSettings = cacheSettings.Value;

            this.CustomMemoryCache = CustomMemoryCache.GetInstance(this.CacheSettings.CacheSize);
        }

        public void UseCache()
        {
            //A bit clunky but as it's just for demonstration purposes i didn't think more elegance was neccessary.
            //Thread sleep is to make times a bit easier to disnguish.

            this.GetCacheResult(this.CustomMemoryCache.AddToCache("SomeKey1", "SomeValue1"));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CustomMemoryCache.AddToCache("SomeKey2", 1));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CustomMemoryCache.AddToCache("SomeKey3", new Student(1001, "Bobby", "Baratheon")));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CustomMemoryCache.AddToCache("SomeKey4", "SomeValue4"));

            this.GetCacheResult(this.CustomMemoryCache.GetItemFromCache("SomeKey4"));
            this.GetCacheResult(this.CustomMemoryCache.GetItemFromCache("someInvalidKey"));
    
            Console.WriteLine("---END---");
            Console.ReadLine();
        }

        private void GetCacheResult(CacheItemResult result)
        {
            if (result.StatusResult.StatusCode == 0)
            {
                Console.WriteLine(result.StatusResult.StatusMessage);
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
