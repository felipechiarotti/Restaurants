
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishForRestaurant
{
    public class DeleteDishForRestaurantCommandHandler(
        ILogger<DeleteDishForRestaurantCommandHandler> logger,
        IDishesRepository dishesRepository,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<DeleteDishForRestaurantCommand>
    {
        public async Task Handle(DeleteDishForRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Deleting dish {DishId} for restaurant: {RestaurantId}", request.Id, request.RestaurantId);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
                throw new ForbidException();

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.Id)
                ?? throw new NotFoundException(nameof(Dish), request.Id.ToString());



            await dishesRepository.DeleteAsync(dish);
        }
    }
}
