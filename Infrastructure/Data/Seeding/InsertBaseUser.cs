using Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Seeding;

internal class InsertBaseUser(UserManager<User> userManager, User user) : ISeeder {
    public async Task Seed(TripDbContext dbContext) {
        if (user == null) {
            throw new Exception("default user can't be null");
        }

        await userManager.CreateAsync(user, "Default123!");
    }
}
