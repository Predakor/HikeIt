using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;

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
        await SeedIfMissing(AppSettingType.ElevationProfile, AddDefaultElevationProfileSetting);
        await SeedIfMissing(AppSettingType.Route, AddDefaultRouteSetting);
        await SeedIfMissing(AppSettingType.Pace, AddDefaultPaceSetting);
        await SeedIfMissing(AppSettingType.Terrain, AddDefaultTerrainSetting);
    }

    private Task<Result<AppSetting>> SeedIfMissing(AppSettingType type, Func<Task<Result<AppSetting>>> addDefault)
    {
        return repository
           .GetBySettingTypeAsync(type, CancellationToken.None)
           .MatchAsync(found => found, error => addDefault())
           .TapAsync(_ => repository.SaveChangesAsync(CancellationToken.None));
    }

    // Same values as the former hard-coded cleanup constants, in production order.
    private Task<Result<AppSetting>> AddDefaultProcessingSetting()
    {
        IReadOnlyList<IPointFilterConfig> filters =
        [
            new MaxSpikeFilter.Config(8),
            new MedianSmoothingFilter.Config(6),
            new EmaSmoothingFilter.Config(0.88),
            new RoundingPrecisionFilter.Config(1),
        ];

        return repository.AddAsync(AppSetting.Create(new GpxDataProcessingSetting(filters)));
    }

    // Same values as the former hard-coded peak matching constants.
    private Task<Result<AppSetting>> AddDefaultPeakMatchingSetting()
    {
        IReadOnlyList<IPeakFilterConfig> filters =
        [
            new LocalMaximaFilter.Config(0),
            new MergeNearbyByDistanceFilter.Config(50),
            new MergeNearbyByElevationFilter.Config(50),
            new PeakLookupFilter.Config(100),
            new MatchWithinDistanceFilter.Config(100),
        ];

        return repository.AddAsync(AppSetting.Create(new PeakMatchingSetting(filters)));
    }

    private Task<Result<AppSetting>> AddDefaultElevationProfileSetting()
    {
        return repository.AddAsync(AppSetting.Create(new ElevationProfileSetting(PipelineDefaults.ElevationProfileFilters)));
    }

    private Task<Result<AppSetting>> AddDefaultRouteSetting()
    {
        return repository.AddAsync(AppSetting.Create(new RouteSetting(PipelineDefaults.RouteFilters)));
    }

    private Task<Result<AppSetting>> AddDefaultPaceSetting()
    {
        return repository.AddAsync(AppSetting.Create(new PaceSetting(PipelineDefaults.PaceFilters)));
    }

    private Task<Result<AppSetting>> AddDefaultTerrainSetting()
    {
        return repository.AddAsync(AppSetting.Create(new TerrainSetting(PipelineDefaults.TerrainFilters)));
    }
}