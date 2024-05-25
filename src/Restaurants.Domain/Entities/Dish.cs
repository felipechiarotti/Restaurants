namespace Restaurants.Domain.Entities;

public class Dish : Entity
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }

    public int? KiloCalories { get; set; }
    public Guid RestaurantId { get; set; }
}
