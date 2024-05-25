using Xunit;
using Restaurants.Application.Tests;
using Moq;
using Restaurants.Domain.Entities;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application.Tests.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandlerTests : FixtureBase
    {
        [Fact()]
        public async Task Handle_ForValidCommand_ReturnsCreatedRestaurant()
        {
            // arrange

            var restaurantId = Guid.NewGuid();
            var createRestaurantCommand = _fixture.Build<CreateRestaurantCommand>().Create();
            var restaurant = new Restaurant();

            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m => m.Map<Restaurant>(createRestaurantCommand)).Returns(restaurant);

            _restaurantRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<Restaurant>()))
                .ReturnsAsync(restaurantId);

            var commandHandler = new CreateRestaurantCommandHandler(
                loggerMock.Object,
                mapperMock.Object,
                _restaurantRepositoryMock.Object,
                _userContextMock.Object);

            // act
            var response = await commandHandler.Handle(createRestaurantCommand, CancellationToken.None);

            // assert
            response.Should().Be(restaurantId);
            restaurant.OwnerId.Should().Be(_currentUserId.ToString());
            mapperMock.Verify(r => r.Map<Restaurant>(createRestaurantCommand), Times.Once());
            _restaurantRepositoryMock.Verify(r => r.CreateAsync(restaurant), Times.Once());
            _userContextMock.Verify(r => r.GetCurrentUser(), Times.Once());
        }
    }
}