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

        // json, not jsonb: jsonb reorders keys, which moves the polymorphic discriminator and breaks reads.
        builder.Property(a => a.JsonValue)
            .HasColumnType("json")
            .IsRequired();

        // Stored by name, so renumbering or reordering the enum can never repoint existing rows.
        builder.Property(a => a.SettingType)
            .HasConversion<string>()
            .HasMaxLength(64);

        builder.HasIndex(a => a.SettingType).IsUnique();

        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}