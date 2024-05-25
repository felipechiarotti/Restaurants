using MediatR;

namespace Restaurants.Application.Dishes.Commands.UpdateDishForRestaurant
{
    public class UpdateDishForRestaurantCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }

        public int? KiloCalories { get; set; }
    }
}
