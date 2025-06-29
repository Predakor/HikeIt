using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfigurations;

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
