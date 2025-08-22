using Domain.Trips.Analytics.ElevationProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips.Analytics.ElevationProfiles;

public class ElevationProfileConfiguration : IEntityTypeConfiguration<ElevationProfile> {
    public void Configure(EntityTypeBuilder<ElevationProfile> builder) {
        builder.OwnsOne(e => e.Start).WithOwner();
    }
}
