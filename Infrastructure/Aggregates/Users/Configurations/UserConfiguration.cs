using Domain.Users;
using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Aggregates.Users.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> builder) {
        builder.HasOne(u => u.Stats).WithOne().HasForeignKey<UserStats>(s => s.Id);
        //add settings later
        //and maybe badges or achievements
    }
}
