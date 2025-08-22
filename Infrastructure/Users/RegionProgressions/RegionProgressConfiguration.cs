using Domain.Users.RegionProgressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace Infrastructure.Users.RegionProgressions;

internal class RegionProgressConfiguration : IEntityTypeConfiguration<RegionProgress> {
    public void Configure(EntityTypeBuilder<RegionProgress> builder) {
        builder.HasKey(rp => rp.Id);

        builder.Property(rp => rp.RegionId).IsRequired();
        builder.Property(rp => rp.TotalReachedPeaks).IsRequired();
        builder.Property(rp => rp.TotalPeaksInRegion).IsRequired();

        //builder.HasOne(rp => rp.User).WithMany().HasForeignKey(rp => rp.UserId);
        builder.HasOne(rp => rp.Region).WithMany().HasForeignKey(rp => rp.RegionId);

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
            )
            .Metadata.SetValueComparer(
                new ValueComparer<Dictionary<int, short>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToDictionary(entry => entry.Key, entry => entry.Value)
                )
            );
    }
}
