using Domain.TripAnalytics.Entities.ElevationProfile;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfigurations;

public class ElevationProfileConfiguration : IEntityTypeConfiguration<ElevationProfile> {
    public void Configure(EntityTypeBuilder<ElevationProfile> builder) {
        builder.OwnsOne(e => e.Start).WithOwner();
    }
}
