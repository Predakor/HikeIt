using Application.Commons.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Shared.Filters;

namespace Application.Trips.Analytics.Pipeline;

// Production defaults of the GPX processing pipeline. A null field in PipelineConfig resolves to these values.
public static class PipelineDefaults
{
    // Same values as GpxDataConfigs.GpxFile (clamp, median, ema, round), in production order.
    public static readonly IReadOnlyList<IPointFilterConfig> CleanupFilters =
    [
        new MaxSpikeFilter.Config(8) with { Value = new MaxSpikeFilter.Config(8) },
        new MedianSmoothingFilter.Config(6) with { Value = new MedianSmoothingFilter.Config(6) },
        new EmaSmoothingFilter.Config(0.88) with { Value = new EmaSmoothingFilter.Config(0.88) },
        new RoundingPrecisionFilter.Config(1) with { Value = new RoundingPrecisionFilter.Config(1) },
    ];

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
