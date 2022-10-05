using Finbourne_MemoryCache.Cache;
using Finbourne_MemoryCache.Models;
using Finbourne_MemoryCache.Models.ExampleClass;
using Microsoft.Extensions.Logging;
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
            //Thread sleep is to make things a bit easier to disnguish.


            //Add Some Items Successfully
            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey1", "SomeValue1"));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey2", 1));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey3", new Student(1001, "Bobby", "Baratheon")));
            Thread.Sleep(3000);
            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey4", "SomeValue4"));
            Thread.Sleep(3000);

            //Add Invalid Items
            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey4", "SomeValue4")); //Duplicate Key
            Thread.Sleep(3000);
            this.GetCacheResult(this.CacheWrapper.AddToCache("", "SomeValue4")); //No Key
            Thread.Sleep(3000);
            this.GetCacheResult(this.CacheWrapper.AddToCache("SomeKey5", null)); //Null object

            //Get Valid Item
            this.GetCacheResult(this.CacheWrapper.GetFromCache("SomeKey4"));
            Thread.Sleep(3000);

            //Get Invalid Items
            this.GetCacheResult(this.CacheWrapper.GetFromCache("someInvalidKey")); //Key not present
            Thread.Sleep(3000);
            this.GetCacheResult(this.CacheWrapper.GetFromCache("")); //Key whitespace
            Thread.Sleep(3000);

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
