using Application.AppSettings;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.AppSettings.Interfaces;
using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline;

public interface IPipelineSettingsService
{
    Task<IReadOnlyList<TFilter>> GetFiltersAsync<TFilter>(StageId stage, CancellationToken cancellationToken)
        where TFilter : IFilterConfig;

    Task<PipelineDefaultsDto> GetDefaultsAsync(CancellationToken cancellationToken);
}

public sealed class PipelineSettingsService(IAppSettingsService appSettingsService) : IPipelineSettingsService
{
    public async Task<IReadOnlyList<TFilter>> GetFiltersAsync<TFilter>(StageId stage, CancellationToken cancellationToken)
        where TFilter : IFilterConfig
    {
        var filters = stage switch
        {
            StageId.Cleanup => await FiltersOf<GpxDataProcessingSetting, IPointFilterConfig>(cancellationToken) as IReadOnlyList<TFilter>,
            StageId.ElevationProfile => await FiltersOf<ElevationProfileSetting, IPointFilterConfig>(cancellationToken) as IReadOnlyList<TFilter>,
            StageId.PeakMatching => await FiltersOf<PeakMatchingSetting, IPeakFilterConfig>(cancellationToken) as IReadOnlyList<TFilter>,
            StageId.Route => await FiltersOf<RouteSetting, IPointFilterConfig>(cancellationToken) as IReadOnlyList<TFilter>,
            StageId.Analytics => null,
            _ => throw new NotImplementedException(),
        };

        return filters ?? [];
    }

    public async Task<PipelineDefaultsDto> GetDefaultsAsync(CancellationToken cancellationToken)
    {
        return new PipelineDefaultsDto(
            await FiltersOf<GpxDataProcessingSetting, IPointFilterConfig>(cancellationToken),
            await FiltersOf<ElevationProfileSetting, IPointFilterConfig>(cancellationToken),
            await FiltersOf<PeakMatchingSetting, IPeakFilterConfig>(cancellationToken),
            await FiltersOf<RouteSetting, IPointFilterConfig>(cancellationToken)
        );
    }

    private async Task<IReadOnlyList<TFilter>> FiltersOf<TSetting, TFilter>(CancellationToken cancellationToken)
        where TSetting : class, IAppSetting<TSetting>, IFilterSetting<TFilter>
    {
        return (await appSettingsService.GetSettingOrDefaultAsync<TSetting>(cancellationToken)).Filters;
    }
}