using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Aggregates.Users;

internal class UserAggregateConfiguration : IAggregateConfiguration {
    public ModelBuilder Apply(ModelBuilder modelBuilder) {
        return modelBuilder
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new UserStatsConfiguration());
    }
}
