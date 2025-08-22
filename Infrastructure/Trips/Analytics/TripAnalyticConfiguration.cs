using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.Peaks;
using Domain.Trips.Analytics.Root;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips.Analytics;

public class TripAnalyticConfiguration : IEntityTypeConfiguration<TripAnalytic> {
    public void Configure(EntityTypeBuilder<TripAnalytic> builder) {
        builder.OwnsOne(a => a.RouteAnalytics).WithOwner();
        builder.OwnsOne(a => a.TimeAnalytics).WithOwner();

        builder.HasOne(a => a.ElevationProfile)
               .WithOne()
               .HasForeignKey<ElevationProfile>(t => t.Id)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.PeaksAnalytic)
               .WithOne()
               .HasForeignKey<PeaksAnalytic>(t => t.Id)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
