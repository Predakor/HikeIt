using Infrastructure.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Data;

public static class MigrationHelper {
    public static async Task MigrateDatabaseAsync(IServiceProvider services) {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;

        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var csvPath = Path.Combine(basePath, "peaks.csv");

        try {
            var context = scopedServices.GetRequiredService<TripDbContext>();
            await context.Database.MigrateAsync();

            var seeder = new Seeder();
            seeder.InsertMountainPeaks(context, csvPath);
        }
        catch (Exception ex) {
            Console.WriteLine($"Migration error: {ex}");
            throw; // Optional: rethrow if you want to stop startup on failure
        }
    }
}
