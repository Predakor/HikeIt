using Domain.Mountains.Peaks;
using System.Collections.Immutable;

namespace Infrastructure.Data.Seeding;

internal class InsertMountainPeaks(ImmutableArray<Peak> peaks) : ISeeder {
    public void Seed(TripDbContext dbContext) {
        if (peaks.Length == 0) {
            throw new Exception("No mountain peaks found in the CSV file.");
        }

        dbContext.Peaks.AddRange(peaks);
    }
}
