using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Aggregates.Mountains.Configuration;

internal static class MountainAggregateConfiguration {
    public static ModelBuilder Apply(ModelBuilder builder) {
        builder.ApplyConfiguration(new RegionConfiguration());
        builder.ApplyConfiguration(new PeakConfiguration());

        return builder;
    }
}
