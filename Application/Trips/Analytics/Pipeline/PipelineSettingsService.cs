using Application.AppSettings;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline;

public interface IPipelineSettingsService : IService
{
    Task<IReadOnlyList<TFilter>> GetFiltersAsync<TFilter>(StageId stage, CancellationToken cancellationToken)
        where TFilter : IFilterConfig;

    Task<Result<PipelineConfig>> GetDefaultsAsync(CancellationToken cancellationToken);
}

public sealed class PipelineSettingsService(IAppSettingsService appSettingsService) : IPipelineSettingsService
{
    public async Task<IReadOnlyList<TFilter>> GetFiltersAsync<TFilter>(
        StageId stage,
        CancellationToken cancellationToken
    )
        where TFilter : IFilterConfig
    {
        var filters = stage switch
        {
            StageId.Cleanup => await FiltersOf<GpxDataProcessingSetting, IPointFilterConfig>(cancellationToken)
                as IReadOnlyList<TFilter>,
            StageId.ElevationProfile => await FiltersOf<ElevationProfileSetting, IPointFilterConfig>(cancellationToken)
                as IReadOnlyList<TFilter>,
            StageId.PeakMatching => await FiltersOf<PeakMatchingSetting, IPeakFilterConfig>(cancellationToken)
                as IReadOnlyList<TFilter>,
            StageId.Route => await FiltersOf<RouteSetting, IPointFilterConfig>(cancellationToken)
                as IReadOnlyList<TFilter>,
            StageId.Terrain => await FiltersOf<TerrainSetting, ITerrainFilterConfig>(cancellationToken)
                as IReadOnlyList<TFilter>,
            StageId.Pace => await FiltersOf<PaceSetting, IPaceFilterConfig>(cancellationToken)
                as IReadOnlyList<TFilter>,
            _ => throw new NotImplementedException(),
        };

        return filters ?? [];
    }

    public async Task<Result<PipelineConfig>> GetDefaultsAsync(CancellationToken cancellationToken)
    {
        return new PipelineConfig(
            new PipeConfig.Cleanup(await GetFiltersAsync<IPointFilterConfig>(StageId.Cleanup, cancellationToken)),
            new PipeConfig.Profile(
                await GetFiltersAsync<IPointFilterConfig>(StageId.ElevationProfile, cancellationToken)
            ),
            new PipeConfig.Peaks(await GetFiltersAsync<IPeakFilterConfig>(StageId.PeakMatching, cancellationToken)),
            new PipeConfig.Route(await GetFiltersAsync<IPointFilterConfig>(StageId.Route, cancellationToken)),
            new PipeConfig.Terrain(await GetFiltersAsync<ITerrainFilterConfig>(StageId.Terrain, cancellationToken)),
            new PipeConfig.Pace(await GetFiltersAsync<IPaceFilterConfig>(StageId.Pace, cancellationToken))
        );
    }

    private async Task<IReadOnlyList<TFilter>> FiltersOf<TSetting, TFilter>(CancellationToken cancellationToken)
        where TSetting : class, IAppSetting<TSetting>, IFilterSetting<TFilter>
    {
        var setting = await appSettingsService.GetSettingOrDefaultAsync<TSetting>(cancellationToken);
        return setting.Filters;
    }
}
