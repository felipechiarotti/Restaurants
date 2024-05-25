using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<Guid> CreateAsync(Dish entity)
    {
        await dbContext.Dishes.AddAsync(entity);
        await SaveChangesAsync();

        return entity.Id;
    }

    public async Task DeleteAsync(Dish entity)
    {
        dbContext.Remove(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(Dish entity)
    {
        dbContext.Dishes.Update(entity);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
        => await dbContext.SaveChangesAsync();
}
