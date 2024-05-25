using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRepository
{
    Task SaveChangesAsync();
}
