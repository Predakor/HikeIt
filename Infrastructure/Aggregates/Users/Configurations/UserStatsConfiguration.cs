using Domain.Users;
using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Aggregates.Users.Configurations;

internal class UserStatsConfiguration : IEntityTypeConfiguration<UserStats> {
    public void Configure(EntityTypeBuilder<UserStats> builder) {
        builder.HasKey(x => x.Id);
        builder.HasOne<User>().WithOne(u => u.Stats).HasForeignKey<UserStats>(s => s.Id);
    }
}
