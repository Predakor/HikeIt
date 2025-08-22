using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Commons.Databases.Seeding.Users;

internal class InsertRoles : ISeeder {
    public static readonly string[] roles = ["Admin", "Moderator", "User", "Demo"];

    readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public InsertRoles(RoleManager<IdentityRole<Guid>> roleManager) {
        _roleManager = roleManager;
    }

    public async Task Seed(TripDbContext dbContext) {
        foreach (var role in roles) {
            if (!await _roleManager.RoleExistsAsync(role)) {
                Console.WriteLine($"Seeding Role: {role}");
                await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }
    }
}
