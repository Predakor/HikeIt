using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data.Seeding.Users;

internal class InsertBaseUser : ISeeder {
    readonly UserManager<User> _userManager;
    readonly User _user;

    public InsertBaseUser(IServiceProvider provider) {
        _userManager = provider.GetRequiredService<UserManager<User>>();
        _user = User.DemoUser;
    }

    public async Task Seed(TripDbContext dbContext) {
        if (_user is null) {
            throw new Exception("default user can't be null");
        }

        var hasNoDemoUser = (await _userManager.FindByNameAsync(_user.UserName!)) == null;
        if (hasNoDemoUser) {
            Console.WriteLine("Seeding default users");
            await _userManager.CreateAsync(_user, "Default123!");
        }
    }
}
