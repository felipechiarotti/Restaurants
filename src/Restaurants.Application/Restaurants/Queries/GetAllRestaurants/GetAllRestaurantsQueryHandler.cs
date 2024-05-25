using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Configurations;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(
    ILogger<GetAllRestaurantsQueryHandler> logger,
    IOptions<ApiSettings> options,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
{
    public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all restaurants {@Request}", request);

        var pageSize = request.PageSize ?? options.Value.PageSize;

        var (restaurants, totalCount) = await restaurantsRepository.GetAllMatchingAsync(
            request.SearchPhrase,
            pageSize,
            request.PageNumber,
            request.SortBy,
            request.SortDirection);

        var restaurantsDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        return new PagedResult<RestaurantDto>(restaurantsDtos, totalCount, pageSize, request.PageNumber);
    }
}
