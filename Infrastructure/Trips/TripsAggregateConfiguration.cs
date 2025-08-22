using Infrastructure.ReachedPeaks;
using Infrastructure.Trips.Analytics;
using Infrastructure.Trips.Analytics.ElevationProfiles;
using Infrastructure.Trips.Analytics.Peaks;
using Infrastructure.Trips.Root;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Trips;

internal static class TripsAggregateConfiguration {
    public static ModelBuilder Apply(ModelBuilder modelBuilder) {
        return modelBuilder
            .ApplyConfiguration(new TripConfiguration())
            .ApplyConfiguration(new ReachedPeakConfiguration())
            .ApplyConfiguration(new TripAnalyticConfiguration())
            .ApplyConfiguration(new PeaksAnalyticConfiguration())
            .ApplyConfiguration(new ElevationProfileConfiguration());
    }
}
