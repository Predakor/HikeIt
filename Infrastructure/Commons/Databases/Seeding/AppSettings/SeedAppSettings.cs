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
        await SeedIfMissing(AppSettingType.GpxDataProcessing, AddDefaultProcessingSetting);
        await SeedIfMissing(AppSettingType.PeakMatching, AddDefaultPeakMatchingSetting);
    }

    private Task SeedIfMissing(AppSettingType type, Func<Task<Result<AppSetting>>> addDefault)
    {
        return repository
           .GetBySettingTypeAsync(type, CancellationToken.None)
           .MatchAsync(found => found, error => addDefault())
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

    // Same values as the former hard-coded peak matching constants.
    private Task<Result<AppSetting>> AddDefaultPeakMatchingSetting()
    {
        return repository.AddAsync(
            AppSetting.Create(
                new PeakMatchingSetting(ProximityRadiusM: 100f, MergeDistanceM: 50f, MergeElevationM: 50f)
            )
        );
    }
}
