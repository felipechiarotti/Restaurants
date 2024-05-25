using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteDishForRestaurant
{
    public class DeleteDishForRestaurantCommand(Guid id, Guid restaurantId) : IRequest
    {
        public Guid Id { get; } = id;
        public Guid RestaurantId { get; } = restaurantId;
    }
}
