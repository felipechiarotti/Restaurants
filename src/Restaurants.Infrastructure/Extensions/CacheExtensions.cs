using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Configuration;
using Restaurants.Infrastructure.Repositories;

namespace Restaurants.Infrastructure.Extensions
{
    public static class CacheExtensions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RedisCacheSettings>(configuration.GetSection("RedisCache"));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetValue<string>("RedisCache:ConnectionString");
            });

            services.AddScoped(typeof(ICacheRepository<>), typeof(CacheRepository<>));
        }
    }
}
