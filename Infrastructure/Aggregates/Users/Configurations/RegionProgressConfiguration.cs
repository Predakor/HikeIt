using Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Infrastructure.Aggregates.Users.Configurations;

internal class RegionProgressConfiguration : IEntityTypeConfiguration<RegionProgress> {
    public void Configure(EntityTypeBuilder<RegionProgress> builder) {
        builder.HasKey(rp => rp.Id);

        builder.Property(rp => rp.UserId).IsRequired();

        builder.Property(rp => rp.RegionId).IsRequired();

        builder.Property(rp => rp.ReachedPeaks).IsRequired();

        builder.Property(rp => rp.TotalPeaks).IsRequired();

        builder
            .Property(rp => rp.PeakVisits)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v =>
                    JsonSerializer.Deserialize<Dictionary<int, short>>(
                        v,
                        (JsonSerializerOptions?)null
                    )!
            );
    }
}
