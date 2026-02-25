using Domain.AppSettings.Root;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.AppSettings;
internal class AppSettingsConfiguration : IEntityTypeConfiguration<AppSetting>
{
    public void Configure(EntityTypeBuilder<AppSetting> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(a => a.JsonValue)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.HasIndex(a => a.SettingType).IsUnique();

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
