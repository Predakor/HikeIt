using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;

namespace Infrastructure.Commons.Databases.Seeding.AppSettings;

internal class SeedAppSettings : ISeeder
{
    private readonly IAppSettingsRepository repository;

    public SeedAppSettings(IAppSettingsRepository repository)
    {
        this.repository = repository;
    }

    public async Task Seed(TripDbContext dbContext)
    {
        await repository
           .GetBySettingTypeAsync(AppSettingType.GpxDataProcessing, CancellationToken.None)
           .MatchAsync(found => found, error => AddDefaultProcessingSetting())
           .TapAsync(_ => repository.SaveChangesAsync(CancellationToken.None));
    }

    private Task<Result<AppSetting>> AddDefaultProcessingSetting()
    {
        return repository.AddAsync(
            AppSetting.Create(
                new GpxDataProcessingSetting(
                    MaxElevationSpike: 8f,
                    EmaSmoothingAlpha: 0.88f,
                    MedianFilterWindowSize: 6,
                    RoundingDecimalsCount: 1
                )
            )
        );
    }
}
