using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler(
    ILogger<UpdateRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IMapper mapper,
    IRestaurantAuthorizationService restaurantAuthorizationService) : IRequestHandler<UpdateRestaurantCommand>
    {
        public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Updating restaurant: {RestaurantId} with {@UpdateRestaurantCommand}", request.Id, request);
            var restaurantEntity = await restaurantsRepository.GetByIdAsync(request.Id) 
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            mapper.Map(request, restaurantEntity);

            if (!restaurantAuthorizationService.Authorize(restaurantEntity, ResourceOperation.Update))
                throw new ForbidException();

            await restaurantsRepository.UpdateAsync(restaurantEntity);
        }
    }
}
