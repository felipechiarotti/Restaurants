using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo
{
    public class UploadRestaurantLogoCommandHandler(
        ILogger<UploadRestaurantLogoCommandHandler> logger,
        IRestaurantsRepository restaurantsRepository,
        IRestaurantAuthorizationService restaurantAuthorizationService,
        IBlobStorageService blobStorageService) : IRequestHandler<UploadRestaurantLogoCommand, string>
    {
        public async Task<string> Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
        {
            var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
                ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
                throw new ForbidException();
             
            restaurant.LogoUrl = await blobStorageService.UploadToBlobAsync(request.File, request.FileName);
            logger.LogInformation("Image uploaded to blob at Url: {Url}", restaurant.LogoUrl);

            await restaurantsRepository.UpdateAsync(restaurant);

            return restaurant.LogoUrl;
        }
    }
}
