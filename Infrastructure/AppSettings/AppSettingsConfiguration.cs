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

        // json, not jsonb: jsonb reorders object keys (by length) on storage, which breaks a cold
        // System.Text.Json polymorphic read of the stored filter configs - their discriminator ends up
        // out of position. json is text-based and preserves the exact property order it was written with.
        builder.Property(a => a.JsonValue)
            .HasColumnType("json")
            .IsRequired();

        builder.HasIndex(a => a.SettingType).IsUnique();

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}