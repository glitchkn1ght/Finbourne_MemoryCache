using Microsoft.Extensions.Logging;
using Finbourne_MemoryCache.CustomCache;
using Finbourne_MemoryCache.Models;
using Finbourne_MemoryCache.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Finbourne_MemoryCache.Models.Config;
using Microsoft.Extensions.Options;
using System.Threading;
using Finbourne_MemoryCache.Models.ExampleClass;

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
            if (result.Error.ErrorCode == 0)
            {
                Console.WriteLine(result.ResultMessage);
            }
            else
            {
                Console.WriteLine($"An Error Occurred: Code={result.Error.ErrorCode}, Message={result.Error.ErrorMessage}");

                if (result.Error.ExceptionMessage != null)
                {
                    this.Logger.LogError($"Operation=GetCacheResult(CustomeCacheOrchestrator), Status=Failure, Message={result.Error.ExceptionMessage}");
                }
            }
        }
    }
}
