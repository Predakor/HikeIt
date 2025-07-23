using Domain.Mountains.Peaks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Aggregates.Mountains.Configuration;

internal class PeakConfiguration : IEntityTypeConfiguration<Peak> {
    public void Configure(EntityTypeBuilder<Peak> builder) {
        builder.Property(e => e.Location).HasColumnType("geography (Point, 4326)");
        builder.HasOne(p => p.Region).WithMany().HasForeignKey(p => p.RegionID);
    }
}
