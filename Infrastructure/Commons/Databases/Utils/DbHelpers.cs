using Infrastructure.Commons.Databases.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Commons.Databases.Utils;

public static class DbHelpers {
    public static async Task TryMigrationAndSeeding(IServiceProvider services) {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        try {
            var context = scopedServices.GetRequiredService<TripDbContext>();
            await context.Database.MigrateAsync();
            await new DataSeeder(scopedServices).Seed(context);
            await context.SaveChangesAsync();
        }
        catch (Exception ex) {
            Console.WriteLine($"Migration error: {ex}");
            throw; // Optional: rethrow if you want to stop startup on failure
        }
    }
}
