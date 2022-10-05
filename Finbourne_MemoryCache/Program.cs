using Finbourne_MemoryCache.Client;
using Finbourne_MemoryCache.Cache;
using Finbourne_MemoryCache.Interfaces;
using Finbourne_MemoryCache.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;

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

            serviceProvider.GetService<CacheClient>().UseCache();
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


            services.AddSingleton<ICustomCache, CustomCache>();  
            services.AddSingleton<ICacheWrapper,CacheWrapper>();

            // add app
            services.AddTransient<CacheClient>();
        }
    }
}






