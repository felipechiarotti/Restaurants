using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

internal class RestaurantSeeder(RestaurantsDbContext dbContext, ILogger<RestaurantSeeder> logger) : IRestaurantSeeder
{
    private const string AdminEmail = "admin@restaurants.com";
    private const string AdminRole = UserRoles.Admin;

    public async Task SeedAsync()
    {
        try
        {
            await ApplyMigrations();
            await SeedAdminUser();
            await SeedRoles();
            await dbContext.SaveChangesAsync();

            await SeedAdminUserRole();
            await SeedRestaurants();

            await dbContext.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }

    private async Task SeedAdminUserRole()
    {
        var user = dbContext.Users.FirstOrDefault(u => u.NormalizedEmail == AdminEmail.ToUpper());

        if (user is null)
            return;

        var role = dbContext.Roles.FirstOrDefault(r => r.NormalizedName == AdminRole.ToUpper());
        if (role is null)
            return;

        var userRole = dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
        if (userRole is null)
        {
            dbContext.UserRoles.Add(new()
            {
                RoleId = role.Id,
                UserId = user.Id
            });
        }
    }

    private async Task SeedAdminUser()
    {
        if (dbContext.Users.FirstOrDefault(u => u.NormalizedEmail == AdminEmail.ToUpper()) is null)
        {
            var adminUser = new User
            {
                Email = AdminEmail,
                UserName = AdminEmail,
                DateOfBirth = new DateOnly(1997, 12, 15),
                Nationality = "Brazilian",
                NormalizedEmail = AdminEmail.ToUpper(),
                NormalizedUserName = AdminEmail.ToUpper(),
                EmailConfirmed = true,
                PasswordHash = "AQAAAAIAAYagAAAAEHuJi2QLGAJrFlkubZ3jkadBQIQaSF56flMuVE/VHb5LV+4bgDI+TP6O8M4h8vjjRw=="
            };
            dbContext.Users.Add(adminUser);
        }


    }
    private async Task ApplyMigrations()
    {
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations is not null && pendingMigrations.Any())
            await dbContext.Database.MigrateAsync();
    }

    private async Task SeedRestaurants()
    {

        if (dbContext.Restaurants.Any())
            return;

        var restaurants = await GetRestaurants();
        await dbContext.Restaurants.AddRangeAsync(restaurants);

    }

    private async Task SeedRoles()
    {
        if (dbContext.Roles.Any())
            return;

        var roles = GetRoles();
        await dbContext.Roles.AddRangeAsync(roles);
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
            [
            new (UserRoles.User),
            new (UserRoles.Owner),
            new (UserRoles.Admin)
            ];

        roles.ForEach(r => r.NormalizedName = r.Name!.ToUpper());

        return roles;
    }
    private async Task<IEnumerable<Restaurant>> GetRestaurants()
    {
        var admin = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == AdminEmail);

        List<Restaurant> restaurants = [
            new()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description =
                    "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes =
                [
                    new ()
                    {
                        Name = "Nashville Hot Chicken",
                        Description = "Nashville Hot Chicken (10 pcs.)",
                        Price = 10.30M,
                    },

                    new ()
                    {
                        Name = "Chicken Nuggets",
                        Description = "Chicken Nuggets (5 pcs.)",
                        Price = 5.30M,
                    },
                ],
                Address = new ()
                {
                    City = "London",
                    Street = "Cork St 5",
                    PostalCode = "WC2N 5DU"
                },
                OwnerId = admin!.Id
            },
            new ()
            {
                Name = "McDonald",
                Category = "Fast Food",
                Description =
                    "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                ContactEmail = "contact@mcdonald.com",
                HasDelivery = true,
                Address = new Address()
                {
                    City = "London",
                    Street = "Boots 193",
                    PostalCode = "W1F 8SR"
                },
                OwnerId = admin!.Id
            }
        ];

        return restaurants;
    }
}

