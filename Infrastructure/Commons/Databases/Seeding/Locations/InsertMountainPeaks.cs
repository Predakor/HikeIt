using Domain.Peaks;
using System.Collections.Immutable;

namespace Infrastructure.Commons.Databases.Seeding.Locations;

internal class InsertMountainPeaks(ImmutableArray<Peak> peaks) : ISeeder {
    public async Task Seed(TripDbContext dbContext) {
        if (peaks.Length == 0) {
            throw new Exception("No mountain peaks found in the CSV file.");
        }
        await dbContext.Peaks.AddRangeAsync(peaks);
    }
}
