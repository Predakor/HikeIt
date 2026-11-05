using Application.AppSettings;
using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Root;
using Application.Trips.Analytics.Pipeline.Runners;

namespace Application.Trips.Analytics.RouteVisualizations;

internal sealed class RouteVisualizationService : IRouteVisualizationService
{
    private readonly IAppSettingsService _appSettingsService;
    private readonly IPointsFilterRunner _pointsRunner;

    public RouteVisualizationService(IAppSettingsService appSettingsService, IPointsFilterRunner pointsRunner)
    {
        _appSettingsService = appSettingsService;
        _pointsRunner = pointsRunner;
    }

    public Task<Result<RoutePath>> GetRouteVisualizationAsync(IEnumerable<GpxPoint> points)
    {
        return _appSettingsService
              .GetSettingAsync<RouteVisualizationSetting>(CancellationToken.None)
              .MapAsync(setting => CreateVisualization(points, setting.FilterConfigs));
    }

    public Result<RoutePath> GetRouteVisualization(IEnumerable<GpxPoint> points, IEnumerable<IPointFilterConfig> configs)
    {
        return CreateVisualization(points, configs);
    }

    private RoutePath CreateVisualization(IEnumerable<GpxPoint> points, IEnumerable<IPointFilterConfig> configs)
    {
        return new RoutePath(_pointsRunner.Apply(points, configs));
    }
}
