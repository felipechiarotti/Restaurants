namespace Restaurants.Domain.Repositories
{
    public interface ICacheRepository<TEntity>
    {
        Task SetAsync(TEntity entity);
        Task<TEntity?> GetAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
