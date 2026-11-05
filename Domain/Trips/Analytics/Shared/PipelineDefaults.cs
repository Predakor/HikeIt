using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Shared.Filters;

namespace Domain.Trips.Analytics.Shared;

public static class PipelineDefaults
{
    public static readonly IReadOnlyList<IPointFilterConfig> CleanupFilters =
    [
        new MaxSpikeFilter.Config(8),
        new MedianSmoothingFilter.Config(6),
        new EmaSmoothingFilter.Config(0.88),
        new RoundingPrecisionFilter.Config(1),
    ];

    public static readonly IReadOnlyList<IPointFilterConfig> ElevationProfileFilters =
    [
        new PinLocalMaximaFilter.Config(true),
        new SimplifyFilter.Config(3),
    ];

    public static readonly IReadOnlyList<IPointFilterConfig> RouteFilters =
    [
        new PinLocalMaximaFilter.Config(true),
        new SimplifyFilter.Config(3),
    ];

    public static readonly IReadOnlyList<IPeakFilterConfig> PeakFilters =
    [
        new LocalMaximaFilter.Config(0),
        new MergeNearbyByDistanceFilter.Config(50),
        new MergeNearbyByElevationFilter.Config(50),
        new PeakLookupFilter.Config(100),
        new MatchWithinDistanceFilter.Config(100),
    ];
}
