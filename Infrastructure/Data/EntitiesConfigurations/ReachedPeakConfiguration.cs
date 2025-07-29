using Domain.ReachedPeaks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfigurations;

public class ReachedPeakConfiguration : IEntityTypeConfiguration<ReachedPeak> {
    public void Configure(EntityTypeBuilder<ReachedPeak> builder) {
        builder.HasOne(rp => rp.Peak)
               .WithMany()
               .HasForeignKey(rp => rp.PeakId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(rp => rp.Trip)
               .WithMany()
               .HasForeignKey(t => t.TripId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.User)
               .WithMany()
               .HasForeignKey(u => u.UserId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
