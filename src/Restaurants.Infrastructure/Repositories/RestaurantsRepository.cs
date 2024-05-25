using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext, ICacheRepository<Restaurant> restaurantCache) : IRestaurantsRepository
{
    public async Task<Guid> CreateAsync(Restaurant entity)
    {
        await dbContext.Restaurants.AddAsync(entity);
        await SaveChangesAsync();

        return entity.Id;
    }

    public async Task DeleteAsync(Restaurant entity)
    {
        dbContext.Remove(entity);
        await restaurantCache.DeleteAsync(entity.Id);
        await SaveChangesAsync();
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
        => await dbContext.Restaurants.ToListAsync();

    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(
        string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
    {
        var searchingPhraseLower = searchPhrase?.ToLower();

        var baseQuery = dbContext.Restaurants
            .Where(r => searchingPhraseLower == null || (r.Name.ToLower().Contains(searchingPhraseLower)
                                                     || r.Description.ToLower().Contains(searchingPhraseLower)));


        var totalCount = await baseQuery.CountAsync();
        
        if(sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r=> r.Name },
                { nameof(Restaurant.Description), r=> r.Description},
                { nameof(Restaurant.Category), r=> r.Category}
            };

            var selectedColumn = columnsSelector[sortBy];
            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var restaurants = await baseQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(Guid id)
    {
        var restaurant = await restaurantCache.GetAsync(id);
        if (restaurant is not null)
            return restaurant;

        restaurant = await dbContext.Restaurants.Include(r => r.Dishes).FirstOrDefaultAsync(r => r.Id == id);
        if (restaurant is null)
            return default;

        await restaurantCache.SetAsync(restaurant);

        return restaurant;
    }

    public async Task UpdateAsync(Restaurant entity)
    {
        dbContext.Restaurants.Update(entity);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
        => await dbContext.SaveChangesAsync();
}
