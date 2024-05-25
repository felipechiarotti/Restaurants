using AutoFixture;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Seeders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Restaurants.API.Tests
{
    public class FixtureBase : IClassFixture<WebApplicationFactory<Program>>
    {
        internal readonly WebApplicationFactory<Program> _factory;
        internal readonly IFixture _fixture;
        internal readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
        internal readonly Mock<IRestaurantSeeder> _restaurantSeederMock = new();

        public FixtureBase(WebApplicationFactory<Program> factory)
        {
            _fixture = new Fixture().Customize(new CompositeCustomization(
                new DateOnlyFixtureCustomization(),
                new TimeOnlyFixtureCustomization()));

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantsRepositoryMock.Object));
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantSeeder), _ => _restaurantSeederMock.Object));
                });
            });
        }
    }
}
