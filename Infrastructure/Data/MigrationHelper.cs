using Infrastructure.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;

public static class MigrationHelper {
    public static async Task MigrateDatabaseAsync(IServiceProvider services) {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;

        try {
            var context = scopedServices.GetRequiredService<TripDbContext>();
            await context.Database.MigrateAsync();
            await DataSeeder.TrySeeding(context, services);
        }
        catch (Exception ex) {
            Console.WriteLine($"Migration error: {ex}");
            throw; // Optional: rethrow if you want to stop startup on failure
        }
    }
}
