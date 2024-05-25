using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository : IRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync();
    Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
    Task<Restaurant?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(Restaurant entity);
    Task DeleteAsync(Restaurant entity);
    Task UpdateAsync(Restaurant entity);
}
