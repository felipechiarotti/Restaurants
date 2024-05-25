using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.UpdateDishForRestaurant
{
    public class UpdateDishForRestaurantCommandHandler(
        ILogger<UpdateDishForRestaurantCommandHandler> logger,
        IMapper mapper, IRestaurantsRepository restaurantsRepository,
        IDishesRepository dishesRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<UpdateDishForRestaurantCommand>
    {
        public async Task Handle(UpdateDishForRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating dish {DishId} for restaurant {RestaurantId}: {@UpdateDishForRestaurantCommand}", request.Id, request.RestaurantId, request);

            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
                throw new ForbidException();

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.Id)
                ?? throw new NotFoundException(nameof(Dish), request.Id.ToString());

            mapper.Map(request, dish);
            await dishesRepository.UpdateAsync(dish);
        }
    }
}
