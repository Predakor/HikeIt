using Domain.Mountains.Regions;

namespace Infrastructure.Data.Seeding;

internal class InsertRegionsAsync(Region[] regions) : ISeeder {
    public async Task Seed(TripDbContext dbContext) {
        if (regions.Length == 0) {
            throw new Exception("No regions found in data seed.");
        }

        await dbContext.Regions.AddRangeAsync(regions);
    }
}
