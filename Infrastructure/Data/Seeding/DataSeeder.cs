using Application.Commons.Options;
using Infrastructure.Data.Loaders;
using Infrastructure.Data.Seeding.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data.Seeding;

internal static class DataSeeder {
    static async Task Seed(TripDbContext dbContext, IServiceScope scope, SeedingOptions options) {
        bool hasNoRegionsInDb = !await dbContext.Regions.AnyAsync();
        if (hasNoRegionsInDb) {
            Console.WriteLine("Seeding regions");
            await new InsertRegionsAsync(DataSeed.Regions).Seed(dbContext);
        }

        var hasNoPeaksInDb = !await dbContext.Peaks.AnyAsync();
        if (hasNoPeaksInDb) {
            Console.WriteLine("Seeding Peaks");

            var resourcePath = options.PeaksUrl;
            Console.WriteLine("Seeding from: " + resourcePath);

            await new InsertMountainPeaks(await PeakCsvLoader.LoadFromLink(resourcePath)).Seed(
                dbContext
            );
        }

        await new InsertRoles(scope.ServiceProvider).Seed(dbContext);
        await new InsertBaseUser(scope.ServiceProvider).Seed(dbContext);
        await new InsertAdminUser(scope.ServiceProvider).Seed(dbContext);
    }

    public static async Task TrySeeding(TripDbContext dbContext, IServiceProvider services) {
        try {
            using var scope = services.CreateScope();

            var seedOptions = scope
                .ServiceProvider.GetRequiredService<IOptions<SeedingOptions>>()
                .Value!;

            await Seed(dbContext, scope, seedOptions);

            await dbContext.SaveChangesAsync();
        }
        catch (Exception exc) {
            Console.WriteLine($"Seeding failed reason  {exc.Message}");
            throw;
        }
    }
}
