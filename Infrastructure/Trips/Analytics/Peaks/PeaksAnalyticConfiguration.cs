using Domain.Trips.Analytics.Peaks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Trips.Analytics.Peaks;

public class PeaksAnalyticConfiguration : IEntityTypeConfiguration<PeaksAnalytic> {
    public void Configure(EntityTypeBuilder<PeaksAnalytic> builder) {

    }
}
