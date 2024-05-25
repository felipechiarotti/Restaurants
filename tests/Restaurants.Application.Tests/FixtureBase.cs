using AutoFixture;
using AutoMapper;
using Moq;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Tests
{
    public class FixtureBase
    {
        internal readonly Guid _currentUserId = Guid.NewGuid();

        internal readonly IFixture _fixture;
        internal readonly IMapper _mapper;

        internal readonly Mock<IUserContext> _userContextMock;
        internal readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;

        public FixtureBase()
        {
            _fixture = new Fixture()
            .Customize(new CompositeCustomization(
                new DateOnlyFixtureCustomization(),
                new TimeOnlyFixtureCustomization()));

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RestaurantsProfile>();
                cfg.AddProfile<DishesProfile>();
            }).CreateMapper();

            _userContextMock = MockUserContext();
            _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        }

        private Mock<IUserContext> MockUserContext()
        {
            var dateOfBirth = new DateOnly(1990, 1, 1);
            var userContextMock = new Mock<IUserContext>();

            userContextMock
                .Setup(x => x.GetCurrentUser())
                .Returns(new CurrentUser(
                    _currentUserId.ToString(),
                    "admin@test.com",
                    [UserRoles.Admin, UserRoles.User],
                    "Brazilian",
                    dateOfBirth));
            return userContextMock;
        }
    }
}
