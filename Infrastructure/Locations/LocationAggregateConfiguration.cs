using Infrastructure.Locations.Peaks;
using Infrastructure.Locations.Regions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Locations;

internal static class LocationAggregateConfiguration {
    public static ModelBuilder Apply(ModelBuilder builder) {
        return builder
            .ApplyConfiguration(new RegionConfiguration())
            .ApplyConfiguration(new PeakConfiguration());
    }
}
