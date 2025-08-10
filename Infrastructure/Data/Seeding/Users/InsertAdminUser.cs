using Application.Commons.Options;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data.Seeding.Users;

internal class InsertAdminUser : ISeeder {
    readonly UserManager<User> _userManager;
    readonly User _defaultAdminUser;
    readonly string _password;

    public InsertAdminUser(IServiceProvider provider) {
        _userManager = provider.GetRequiredService<UserManager<User>>();

        var userJsons =
            provider.GetRequiredService<IOptions<SeedingOptions>>().Value.Users
            ?? throw new Exception("default admin seeding data is null");

        var admin = userJsons.GetAdmin;

        _defaultAdminUser = new User() {
            Id = Guid.Parse(admin.Id.ToString()),
            UserName = admin.UserName,
            FirstName = admin.FirstName,
            LastName = admin.LastName,
            Email = admin.Email,
            EmailConfirmed = true,
        };

        _password = admin.Password;
    }

    public async Task Seed(TripDbContext dbContext) {
        var userExists = await _userManager.FindByIdAsync(_defaultAdminUser.Id.ToString());
        if (userExists is not null) {
            return;
        }

        var result = await _userManager.CreateAsync(_defaultAdminUser, _password);
        if (result.Errors.Any()) {
            throw new Exception(
                "Failed to create admin user reason" + result.Errors.First().Description
            );
        }

        var assignRoleResult = await _userManager.AddToRoleAsync(_defaultAdminUser, "Admin");
        if (assignRoleResult.Errors.Any()) {
            throw new Exception(
                "Failed to assign to adming role reason: " + result.Errors.First().Description
            );
        }
    }
}
