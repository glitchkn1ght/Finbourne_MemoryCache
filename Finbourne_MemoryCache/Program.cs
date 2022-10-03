using System;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Finbourne_MemoryCache.Models;
using Finbourne_MemoryCache.Interfaces;
using Finbourne_MemoryCache.CustomCache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using Finbourne_MemoryCache.BusinessLogic;
using Finbourne_MemoryCache.Models.Config;

namespace Finbourne_MemoryCache
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            // create service provider
            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<CustomeCache_Client>().UseCache();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // build config
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\")))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

            // configure logging
            services.AddLogging(builder => builder.AddSerilog(
                new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger()))
                .BuildServiceProvider();


            //Configure cache settings
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

            // add app
            services.AddTransient<CustomeCache_Client>();
        }

    }
}






