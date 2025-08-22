using Domain.Users.Root;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Commons.Databases.Seeding.Users;

internal class InsertBaseUser : ISeeder {
    readonly UserManager<User> _userManager;
    readonly User _user;

    public InsertBaseUser(UserManager<User> manager) {
        _userManager = manager;

        _user = new User() {
            Id = User.DemoUser.Id,
            Email = User.DemoUser.Email,
            UserName = User.DemoUser.UserName,
            FirstName = User.DemoUser.FirstName,
            LastName = User.DemoUser.LastName,
            EmailConfirmed = true,
        };
    }

    public async Task Seed(TripDbContext dbContext) {
        if (_user is null) {
            throw new Exception("default user can't be null");
        }

        var hasNoDemoUser = await _userManager.FindByNameAsync(_user.UserName!) == null;
        if (hasNoDemoUser) {
            Console.WriteLine("Seeding default users");
            await _userManager.CreateAsync(_user, "Default123!");
        }

        await _userManager.AddToRoleAsync(_user, "Demo");
    }
}
