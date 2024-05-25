using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using System.Security.Claims;
using Xunit;

namespace Restaurants.Application.Tests.Users
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            // arrange
            var dateOfBirth = new DateOnly(1990, 1, 1);
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, "1"),
                new(ClaimTypes.Email, "test@test.com"),
                new(ClaimTypes.Role, UserRoles.Admin),
                new(ClaimTypes.Role, UserRoles.User),
                new("Nationality", "Brazilian"),
                new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext()
            {
                User = user
            });

            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act
            var currentUser = userContext.GetCurrentUser();

            // asset
            currentUser.Should().NotBeNull();
            currentUser!.Id.Should().Be(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            currentUser.Email.Should().Be(claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value);
            currentUser.Nationality.Should().Be(claims.FirstOrDefault(c => c.Type == "Nationality")!.Value);
            currentUser.DateOfBirth.Should().Be(dateOfBirth);
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
        }

        [Fact()]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {
            // arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act
            Action action = () => userContext.GetCurrentUser();
            action.Should().Throw<InvalidOperationException>();

        }
    }
}