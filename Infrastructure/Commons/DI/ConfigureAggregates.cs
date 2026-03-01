using Infrastructure.AppSettings;
using Infrastructure.Locations;
using Infrastructure.Trips;
using Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Commons.DI;

internal static class ConfigureAggregates
{
    public static ModelBuilder ApplyAggregatesConfigurations(this ModelBuilder modelBuilder)
    {
        TripsAggregateConfiguration.Apply(modelBuilder);
        LocationAggregateConfiguration.Apply(modelBuilder);
        UserAggregateConfiguration.Apply(modelBuilder);
        modelBuilder.ApplyConfiguration(new AppSettingsConfiguration());
        return modelBuilder;
    }
}
