using Infrastructure.Data.Loaders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Seeding;

internal class DataSeeder(TripDbContext dbContext) {
    public async Task TrySeeding() {
        try {
            if (!await dbContext.Regions.AnyAsync()) {
                Console.WriteLine("Seeding regions");
                new InsertRegionsAsync(DataSeed.Regions).Seed(dbContext);
            }

            if (!await dbContext.Peaks.AnyAsync()) {
                Console.WriteLine("Seeding Peaks");

                var resourcePath = Path.Combine(AppContext.BaseDirectory, "peaks.csv");
                Console.WriteLine("Path for seeding source: " + resourcePath);

                new InsertMountainPeaks(PeakCsvLoader.LoadFrom(resourcePath)).Seed(dbContext);
            }
            await dbContext.SaveChangesAsync();
        }
        catch (Exception exc) {
            Console.WriteLine($"Seeding failed reason  {exc.Message}");
            throw;
        }
    }
}
