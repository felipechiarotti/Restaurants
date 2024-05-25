using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IDishesRepository : IRepository
{
    Task<Guid> CreateAsync(Dish entity);
    Task DeleteAsync(Dish entity);
    Task UpdateAsync(Dish entity);
}
