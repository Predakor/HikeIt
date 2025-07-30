using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Aggregates.Users.Configurations;

internal class UserAggregateConfiguration : IAggregateConfiguration {
    public ModelBuilder Apply(ModelBuilder modelBuilder) {
        return modelBuilder
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new UserStatsConfiguration())
            .ApplyConfiguration(new RegionProgressConfiguration());
    }
}
