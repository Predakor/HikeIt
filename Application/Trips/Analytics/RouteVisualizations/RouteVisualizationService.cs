using Application.AppSettings;
using Application.Commons.Abstractions;
using Domain.AppSettings.Root;
using Domain.AppSettings.Settings;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Shared.Filters;
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
              .GetSettingAsync<RouteVisualizationSetting>(AppSettingType.RouteVisualization, CancellationToken.None)
              .MapAsync(setting => GpxFilterFactory.CreateFilterList(setting.FilterConfigs))
              .MapAsync(filters => CreateVisualization(points, filters));

    }
    public Result<RoutePath> GetRouteVisualization(IEnumerable<GpxPoint> points, IEnumerable<IFilterConfig> configs)
    {
        return CreateVisualization(
            points,
            GpxFilterFactory.CreateFilterList(configs)
        );

    }

    private static RoutePath CreateVisualization(IEnumerable<GpxPoint> points, IEnumerable<IFilter<MutableGpxPoint>> filterList)
    {
        var muttablePoints = points.Select(p => p.ToMutable()).ToList();

        foreach (var filter in filterList)
        {
            filter.Apply(muttablePoints);
        }

        return new RoutePath(muttablePoints);
    }

    public static class GpxFilterFactory
    {
        public static IFilter<MutableGpxPoint> CreateFilter(IFilterConfig config)
        {
            return config switch
            {
                MedianSmoothingFilter.Config c => new MedianSmoothingFilter(c.Value),
                MaxSpikeFilter.Config c => new MaxSpikeFilter(c.Value),
                EmaSmoothingFilter.Config c => new EmaSmoothingFilter(c.Value),
                RoundingPrecisionFilter.Config c => new RoundingPrecisionFilter(c.Value),
                _ => throw new NotSupportedException($"Filter type {config.GetType().Name} is not supported.")
            };
        }

        public static IList<IFilter<MutableGpxPoint>> CreateFilterList(IEnumerable<IFilterConfig> configs)
        {
            return [.. configs.Select(CreateFilter)];
        }
    }

}