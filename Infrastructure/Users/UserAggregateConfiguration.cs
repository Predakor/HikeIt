using Infrastructure.Users.RegionProgressions;
using Infrastructure.Users.Root;
using Infrastructure.Users.Stats;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users;

internal class UserAggregateConfiguration {
    public static ModelBuilder Apply(ModelBuilder modelBuilder) {
        return modelBuilder
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new UserStatsConfiguration())
            .ApplyConfiguration(new RegionProgressConfiguration());
    }
}
