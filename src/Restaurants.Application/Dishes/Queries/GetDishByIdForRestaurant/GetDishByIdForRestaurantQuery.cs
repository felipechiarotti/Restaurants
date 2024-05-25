using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant
{
    public class GetDishByIdForRestaurantQuery(Guid id, Guid restaurantId) : IRequest<DishDto>
    {
        public Guid Id { get; } = id;
        public Guid RestaurantId { get; } = restaurantId;
    }
}
