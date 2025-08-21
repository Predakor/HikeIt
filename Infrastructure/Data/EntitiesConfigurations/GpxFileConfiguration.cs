using Domain.FileReferences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.EntitiesConfigurations;

public class GpxFileConfiguration : IEntityTypeConfiguration<FileReference> {
    public void Configure(EntityTypeBuilder<FileReference> builder) {
        builder.HasKey(g => g.Id);
    }
}
