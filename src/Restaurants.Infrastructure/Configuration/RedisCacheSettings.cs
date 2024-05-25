namespace Restaurants.Infrastructure.Configuration
{
    public class RedisCacheSettings
    {
        public string ConnectionString { get; set; } = default!;
        public int AbsoluteExpiration { get; set; } = default!;
    }
}
