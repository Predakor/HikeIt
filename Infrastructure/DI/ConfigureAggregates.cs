using Infrastructure.Aggregates.Mountains.Configuration;
using Infrastructure.Aggregates.Users.Configurations;
using Infrastructure.Data.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DI;

public static class ConfigureAggregates {
    public static ModelBuilder ApplyAggregatesConfigurations(this ModelBuilder modelBuilder) {
        TripsAggregateConfiguration.Apply(modelBuilder);
        MountainAggregateConfiguration.Apply(modelBuilder);
        new UserAggregateConfiguration().Apply(modelBuilder);
        return modelBuilder;
    }
}
