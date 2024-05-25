using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantsById;

public class GetRestaurantsByIdQueryHandler(
    ILogger<GetRestaurantsByIdQueryHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository,
    IBlobStorageService blobStorageService) : IRequestHandler<GetRestaurantsByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantsByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting restaurant {RestaurantId}", request.Id);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

        var restaurantDto = mapper.Map<RestaurantDto>(restaurant);
        restaurantDto.LogoUrl = blobStorageService.GetBlobSasUrl(restaurant.LogoUrl);
        return restaurantDto;
    }
}
