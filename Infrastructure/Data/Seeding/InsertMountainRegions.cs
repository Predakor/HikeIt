using Domain.Mountains.Regions;

namespace Infrastructure.Data.Seeding;

internal class InsertRegionsAsync(Region[] regions) : ISeeder {
    public void Seed(TripDbContext dbContext) {
        if (regions.Length == 0) {
            throw new Exception("No regions found in data seed.");
        }

        dbContext.Regions.AddRange(regions);
    }
}
