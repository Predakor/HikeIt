using Domain.TripAnalytics;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfigurations;

public class TripConfiguration : IEntityTypeConfiguration<Trip> {
    public void Configure(EntityTypeBuilder<Trip> builder) {
        builder
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(t => t.Region)
            .WithMany()
            .HasForeignKey(t => t.RegionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(t => t.GpxFile)
            .WithOne()
            .HasForeignKey<GpxFile>(t => t.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(t => t.Analytics)
            .WithOne()
            .HasForeignKey<TripAnalytic>(t => t.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(t => t.Peaks)
            .WithOne(rp => rp.Trip)
            .HasForeignKey(rp => rp.TripId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
