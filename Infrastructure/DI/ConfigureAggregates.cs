using Infrastructure.Data.EntitiesConfigurations;
using Infrastructure.Mountains.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DI;
public static class ConfigureAggregates {

    public static ModelBuilder ApplyAggregatesConfigurations(this ModelBuilder modelBuilder) {
        TripsAggregateConfiguration.Apply(modelBuilder);
        MountainAggregateConfiguration.Apply(modelBuilder);

        return modelBuilder;
    }

}