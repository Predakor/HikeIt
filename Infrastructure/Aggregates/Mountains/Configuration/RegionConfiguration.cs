using Domain.Mountains.Regions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Aggregates.Mountains.Configuration;

internal class RegionConfiguration : IEntityTypeConfiguration<Region> {
    public void Configure(EntityTypeBuilder<Region> builder) {
    }
}
