using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfigurations;

public class PeaksAnalyticConfiguration : IEntityTypeConfiguration<PeaksAnalytic> {
    public void Configure(EntityTypeBuilder<PeaksAnalytic> builder) {

    }
}
