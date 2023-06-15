
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web2023_BE.Extension;

namespace Web2023_BE.Cache.Redis
{
    public static class RedisCacheFactory
    {
        public static void InjectDistCacheService(IServiceCollection services, IConfiguration configuration)
        {
            var config = ExtensionFactory.InjectConfig<RedisCacheConfig>(configuration, "RedisCache", services);
            services.AddSingleton<IDistCached, RedisCached>();
        }
    }
}
