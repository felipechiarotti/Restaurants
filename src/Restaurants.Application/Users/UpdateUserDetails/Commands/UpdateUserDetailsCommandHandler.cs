using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.UpdateUserDetails.Commands
{
    public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommand> logger,
        IUserContext userContext,
        IUserStore<User> userStore) : IRequestHandler<UpdateUserDetailsCommand>
    {
        public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();

            logger.LogInformation("Updating user: {UserId}, with {@UpdateUserDetailsCommand}", user!.Id, request);

            var dbUser = await userStore.FindByIdAsync(user!.Id.ToString(), cancellationToken) ??
                throw new NotFoundException(nameof(User), user!.Id.ToString());

            dbUser.Nationality = request.Nationality;
            dbUser.DateOfBirth = request.DateOfBirth;

            await userStore.UpdateAsync(dbUser, cancellationToken);
        }
    }
}
