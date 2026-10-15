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
            muttablePoints = [.. filter.Apply(muttablePoints)];
        }

        return new RoutePath(muttablePoints);
    }

    public static class GpxFilterFactory
    {
        private static readonly Dictionary<Type, Func<IFilterConfig, IFilter<MutableGpxPoint>>> FilterHandlers = new()
        {
            [typeof(MedianSmoothingFilter.Config)] = Create<MedianSmoothingFilter.Config>(value => new MedianSmoothingFilter(value)),
            [typeof(MaxSpikeFilter.Config)] = Create<MaxSpikeFilter.Config>(value => new MaxSpikeFilter(value)),
            [typeof(EmaSmoothingFilter.Config)] = Create<EmaSmoothingFilter.Config>(value => new EmaSmoothingFilter(value)),
            [typeof(RoundingPrecisionFilter.Config)] = Create<RoundingPrecisionFilter.Config>(value => new RoundingPrecisionFilter(value)),
            [typeof(PinLocalMaximaFilter.Config)] = Create<PinLocalMaximaFilter.Config>(value => new PinLocalMaximaFilter(value)),
            [typeof(SimplifyFilter.Config)] = Create<SimplifyFilter.Config>(value => new SimplifyFilter(value)),
            [typeof(CoordinateSmoothingFilter.Config)] = Create<CoordinateSmoothingFilter.Config>(value => new CoordinateSmoothingFilter(value)),
        };

        public static IFilter<MutableGpxPoint> CreateFilter(IFilterConfig config)
        {
            if (!FilterHandlers.TryGetValue(config.GetType(), out var handler))
            {
                throw new NotSupportedException($"Filter type {config.GetType().Name} is not supported.");
            }

            return handler(config);
        }

        // Wraps a typed filter constructor so the handler map can take the untyped config.
        private static Func<IFilterConfig, IFilter<MutableGpxPoint>> Create<TValue>(Func<TValue, IFilter<MutableGpxPoint>> create)
        {
            return config => create((TValue)config.GetValue());
        }

        public static IList<IFilter<MutableGpxPoint>> CreateFilterList(IEnumerable<IFilterConfig> configs)
        {
            return [.. configs.Select(CreateFilter)];
        }
    }

}