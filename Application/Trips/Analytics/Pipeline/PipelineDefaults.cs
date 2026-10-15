using System.Globalization;
using Application.Commons.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Root.Builders.Config;

namespace Application.Trips.Analytics.Pipeline;

// Production defaults of the GPX processing pipeline. A null field in PipelineConfig resolves to these values.
public static class PipelineDefaults
{
    // Same values as GpxDataConfigs.GpxFile (clamp, median, ema, round), in production order.
    public static readonly IReadOnlyList<IPointFilterConfig> CleanupFilters = CleanupFiltersFrom(GpxDataConfigs.GpxFile);

    // Cleanup filters in production order, built from the GPX processing settings (DB row or the code constants).
    public static IReadOnlyList<IPointFilterConfig> CleanupFiltersFrom(IGpxDataProcessingSettings settings)
    {
        var maxSpike = AsDouble(settings.MaxElevationSpike);
        var ema = AsDouble(settings.EmaSmoothingAlpha);

        return
        [
            new MaxSpikeFilter.Config(maxSpike) with { Value = new MaxSpikeFilter.Config(maxSpike) },
            new MedianSmoothingFilter.Config(settings.MedianFilterWindowSize) with { Value = new MedianSmoothingFilter.Config(settings.MedianFilterWindowSize) },
            new EmaSmoothingFilter.Config(ema) with { Value = new EmaSmoothingFilter.Config(ema) },
            new RoundingPrecisionFilter.Config(settings.RoundingDecimalsCount) with { Value = new RoundingPrecisionFilter.Config(settings.RoundingDecimalsCount) },
        ];
    }

    // Goes through the float's shortest decimal form, so 0.88f becomes the double 0.88 used by the code constants.
    static double AsDouble(float value) => double.Parse(value.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);

    // Production stores every cleaned point as the route (TripAnalyticService builds the RoutePath from all points), so no filters.
    public static readonly IReadOnlyList<IPointFilterConfig> RouteFilters = [];

    // Same values as the former ReachedPeakService.WithProximityMerge() call, in production order.
    public static readonly IReadOnlyList<IPeakFilterConfig> PeakFilters =
    [
        new MergeNearbyByDistanceFilter.Config(50) with { Value = new MergeNearbyByDistanceFilter.Config(50) },
        new MergeNearbyByElevationFilter.Config(50) with { Value = new MergeNearbyByElevationFilter.Config(50) },
    ];

    // Lookup radius for matching candidate peaks in the database. Scalar, because the lookup is not a filter.
    public const float PeakProximityRadiusM = 100f;
}

public static class PeakFilterFactory
{
    static readonly Dictionary<Type, Func<IFilterConfig, IFilter<GpxPointWithDistance>>> Factories = new()
    {
        [typeof(MergeNearbyByDistanceFilter.Config)] = config =>
            new MergeNearbyByDistanceFilter(((MergeNearbyByDistanceFilter.Config)config).Value),
        [typeof(MergeNearbyByElevationFilter.Config)] = config =>
            new MergeNearbyByElevationFilter(((MergeNearbyByElevationFilter.Config)config).Value),
    };

    public static IList<GpxPointWithDistance> Apply(
        IList<GpxPointWithDistance> candidates,
        IEnumerable<IPeakFilterConfig> configs
    )
    {
        IList<GpxPointWithDistance> result = candidates;

        foreach (var config in configs)
        {
            var filter = Factories[config.GetType()](config);
            result = filter.Apply(result);
        }

        return result;
    }
}
