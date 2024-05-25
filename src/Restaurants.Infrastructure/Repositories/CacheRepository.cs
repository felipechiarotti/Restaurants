using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Configuration;
using System.Text.Json;

namespace Restaurants.Infrastructure.Repositories
{
    public class CacheRepository<TEntity>(IDistributedCache distributedCache, ILogger<CacheRepository<TEntity>> logger, IOptions<RedisCacheSettings> redisOptions) : ICacheRepository<TEntity>
        where TEntity : Entity
    {
        private readonly RedisCacheSettings _options = redisOptions.Value;

        public async Task SetAsync(TEntity entity)
        {
            var serializedEntity = JsonSerializer.Serialize(entity);
            await distributedCache.SetStringAsync(
                GetKey(entity.Id),
                serializedEntity,
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(_options.AbsoluteExpiration)
                });
            
            logger.LogInformation("{EntityType} {EntityId} saved on Cache: {@Entity}", typeof(TEntity), entity.Id, entity);
        }

        public async Task<TEntity?> GetAsync(Guid id)
        {
            var deserializedEntity = await distributedCache.GetStringAsync(GetKey(id));
            if (string.IsNullOrEmpty(deserializedEntity))
                return default;

            var entity = JsonSerializer.Deserialize<TEntity>(deserializedEntity);
            logger.LogInformation("{EntityType} {EntityId} retrieved from Cache: {@Entity}", typeof(TEntity), id, entity);

            return entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            await distributedCache.RemoveAsync(GetKey(id));
            logger.LogInformation("{EntityType} {EntityId} deleted from Cache", typeof(TEntity), id);
        }

        private string GetKey(Guid id)
            => $"{typeof(TEntity).Name}:{id}";
    }

    public class CacheOptions
    {
        public TimeSpan Ttl { get; set; }
    }
}
