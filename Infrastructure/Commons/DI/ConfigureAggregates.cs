using Infrastructure.Locations;
using Infrastructure.Trips;
using Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commons.DI;

internal static class ConfigureAggregates {
    public static ModelBuilder ApplyAggregatesConfigurations(this ModelBuilder modelBuilder) {
        TripsAggregateConfiguration.Apply(modelBuilder);
        LocationAggregateConfiguration.Apply(modelBuilder);
        UserAggregateConfiguration.Apply(modelBuilder);
        return modelBuilder;
    }
}
