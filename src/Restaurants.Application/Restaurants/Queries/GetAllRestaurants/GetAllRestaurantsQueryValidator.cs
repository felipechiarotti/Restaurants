using FluentValidation;
using Microsoft.Extensions.Options;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Configurations;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
    {
        private string[] allowedSortyByColumnNames = [
            nameof(RestaurantDto.Name),
            nameof(RestaurantDto.Description),
            nameof(RestaurantDto.Category)];

        public GetAllRestaurantsQueryValidator(IOptions<ApiSettings> options)
        {
            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(r => r.PageSize)
                .LessThanOrEqualTo(options.Value.MaxPagesize);

            RuleFor(r => r.SortBy)
                .Must(value => allowedSortyByColumnNames.Contains(value))
                .When(q => q.SortBy != null)
                .WithMessage($"Sort by is options, or must be in [{string.Join(", ",allowedSortyByColumnNames)}]");
        }
    }
}
