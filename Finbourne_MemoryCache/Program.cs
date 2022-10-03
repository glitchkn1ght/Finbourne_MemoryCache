using System;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Finbourne_MemoryCache.Models;
using Finbourne_MemoryCache.Interfaces;
using Finbourne_MemoryCache.CustomCache;

namespace Finbourne_MemoryCache
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            CustomMemoryCache cache = CustomMemoryCache.GetInstance(2);

            cache.AddToCache("SomeKey1", "SomeValue1");
            Thread.Sleep(3000);
            //CustomMemoryCache.AddToCache("SomeKey2", "SomeValue2");
            //CustomMemoryCache.AddToCache("SomeKey3", "SomeValue3");
            cache.GetItemFromCache("someInvalidKey");

            Console.WriteLine("Successfully Added to cache");
            Console.ReadLine();

            //var cachedObject = CustomMemoryCache.RetrieveFromCache("SomeKey");
        }
    }

  
}




