using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantsById;

public class GetRestaurantsByIdQuery(Guid id) : IRequest<RestaurantDto>
{
    public Guid Id { get; } = id;
}
