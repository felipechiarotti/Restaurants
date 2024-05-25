using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization.Policy;
using Restaurants.API.Tests;
using System.Net;
using System.Net.Http.Json;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using AutoFixture;
using Moq;

namespace Restaurants.API.Tests.Controllers
{
    public class RestaurantsControllerTests : FixtureBase
    {
        private readonly Guid _restaurantId = Guid.NewGuid();
        public RestaurantsControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {
            var restaurant = _fixture.Build<Restaurant>().With(r => r.Id, _restaurantId).Without(r => r.Owner).Create();
            _restaurantsRepositoryMock.Setup(r => r.GetByIdAsync(restaurant.Id))
                .ReturnsAsync(restaurant);
        }

        [Fact()]
        public async void GetAll_ForValidRequest_Returns200Ok()
        {
            //arrange
            var client = _factory.CreateClient();

            var result = await client.GetAsync("/api/restaurants?pageNumber=1");

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact()]
        public async Task GetAll_ForInvalidRequest_Returns400Ok()
        {
            //arrange
            var client = _factory.CreateClient();

            var result = await client.GetAsync("/api/restaurants");
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact()]
        public async void GetById_ForValidRequest_Returns200Ok()
        {
            //arrange
            var client = _factory.CreateClient();

            var result = await client.GetAsync($"/api/restaurants/{_restaurantId}");
            var response = await result.Content.ReadFromJsonAsync<RestaurantDto>();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            response!.Id.Should().Be(_restaurantId);
        }
    }
}