using Domain.Users;
using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Aggregates.Users.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        builder
            .HasOne(u => u.Stats)
            .WithOne()
            .HasForeignKey<UserStats>(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(u => u.RegionProgresses)
            .WithOne(rp => rp.User)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
