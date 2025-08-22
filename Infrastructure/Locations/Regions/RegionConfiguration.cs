using Domain.Locations.Regions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Locations.Regions;

internal class RegionConfiguration : IEntityTypeConfiguration<Region> {
    public void Configure(EntityTypeBuilder<Region> builder) {
    }
}
