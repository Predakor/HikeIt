using Application.AppSettings;
using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Shared;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Application.Trips.Analytics.RouteVisualizations;

internal sealed class RouteVisualizationService : IRouteVisualizationService
{
    private readonly IAppSettingsService _appSettingsService;

    public RouteVisualizationService(IAppSettingsService appSettingsService)
    {
        _appSettingsService = appSettingsService;
    }

    public Task<Result<RoutePath>> GetRouteVisualizationAsync(IEnumerable<GpxPoint> points)
    {
        return _appSettingsService
              .GetSettingAsync<RouteVisualizationSetting>(CancellationToken.None)
              .MapAsync(setting => PointFilters.CreateFilterList(setting.FilterConfigs))
              .MapAsync(filters => CreateVisualization(points, filters));

    }
    public Result<RoutePath> GetRouteVisualization(IEnumerable<GpxPoint> points, IEnumerable<IFilterConfig> configs)
    {
        return CreateVisualization(
            points,
            PointFilters.CreateFilterList(configs)
        );

    }

    private static RoutePath CreateVisualization(IEnumerable<GpxPoint> points, IEnumerable<IFilter<MutableGpxPoint>> filterList)
    {
        var muttablePoints = points.Select(p => p.ToMutable()).ToList();

        foreach (var filter in filterList)
        {
            muttablePoints = [.. filter.Apply(muttablePoints)];
        }

        return new RoutePath(muttablePoints);
    }
}
