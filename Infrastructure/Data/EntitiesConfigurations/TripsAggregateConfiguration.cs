using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.EntitiesConfigurations;

internal static class TripsAggregateConfiguration {
    public static ModelBuilder Apply(ModelBuilder modelBuilder) {
        modelBuilder
            .ApplyConfiguration(new GpxFileConfiguration())
            .ApplyConfiguration(new TripConfiguration())
            .ApplyConfiguration(new ReachedPeakConfiguration())
            .ApplyConfiguration(new TripAnalyticConfiguration())
            .ApplyConfiguration(new PeaksAnalyticConfiguration())
            .ApplyConfiguration(new ElevationProfileConfiguration());

        return modelBuilder;
    }
}
