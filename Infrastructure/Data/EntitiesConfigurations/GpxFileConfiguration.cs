using Domain.Trips.Entities.GpxFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfigurations;

public class GpxFileConfiguration : IEntityTypeConfiguration<GpxFile> {
    public void Configure(EntityTypeBuilder<GpxFile> builder) {
        builder.HasKey(g => g.Id);
    }
}
