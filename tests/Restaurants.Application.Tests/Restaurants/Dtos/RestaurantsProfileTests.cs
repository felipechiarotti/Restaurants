using AutoFixture;
using FluentAssertions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Tests;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Tests.Restaurants.Dtos
{
    public class RestaurantsProfileTests : FixtureBase
    {
        [Fact()]
        public void CreateMap_FromRestaurantToRestaurantDto_MapsCorrectly()
        {
            // arrange
            var restaurant = _fixture.Build<Restaurant>()
                .Without(r => r.Owner)
                .Without(r => r.Dishes)
                .Create();

            //act
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            restaurantDto.Should().NotBeNull();
            restaurantDto.Id.Should().Be(restaurant.Id);
            restaurantDto.Name.Should().Be(restaurant.Name);
            restaurantDto.Description.Should().Be(restaurant.Description);
            restaurantDto.Category.Should().Be(restaurant.Category);
            restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
            restaurantDto.City.Should().Be(restaurant.Address!.City);
            restaurantDto.Street.Should().Be(restaurant.Address!.Street);
            restaurantDto.PostalCode.Should().Be(restaurant.Address!.PostalCode);
        }

        [Fact()]
        public void CreateMap_FromCreateRestaurantCommandToRestaurant_MapsCorrectly()
        {
            // arrange
            var createRestaurantCommand = _fixture.Build<CreateRestaurantCommand>()
                .Create();

            //act
            var restaurant = _mapper.Map<Restaurant>(createRestaurantCommand);

            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(createRestaurantCommand.Name);
            restaurant.Description.Should().Be(createRestaurantCommand.Description);
            restaurant.Category.Should().Be(createRestaurantCommand.Category);
            restaurant.HasDelivery.Should().Be(createRestaurantCommand.HasDelivery);
            restaurant.Address.City.Should().Be(restaurant.Address!.City);
            restaurant.Address.Street.Should().Be(restaurant.Address!.Street);
            restaurant.Address.PostalCode.Should().Be(restaurant.Address!.PostalCode);
        }

        [Fact()]
        public void CreateMap_FromUpdateRestaurantCommandToRestaurant_MapsCorrectly()
        {
            // arrange
            var updateRestaurantCommand = _fixture.Build<UpdateRestaurantCommand>()
                .Create();

            //act
            var restaurant = _mapper.Map<Restaurant>(updateRestaurantCommand);

            restaurant.Should().NotBeNull();
            restaurant.Name.Should().Be(updateRestaurantCommand.Name);
            restaurant.Description.Should().Be(updateRestaurantCommand.Description);
            restaurant.HasDelivery.Should().Be(updateRestaurantCommand.HasDelivery);
        }
    }
}