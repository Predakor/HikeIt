using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Route.Filters;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Analytics.Time.Filters;

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
        new SimplifyFilter.Config(6),
        new RoundingPrecisionFilter.Config(0),
    ];

    public static readonly IReadOnlyList<IPointFilterConfig> RouteFilters =
    [
        new PinLocalMaximaFilter.Config(true),
        new SimplifyFilter.Config(4),
        new EmaSmoothingFilter.Config(0.92, PointFields.Coordinates),
        new RoundingPrecisionFilter.Config(6, PointFields.Coordinates),
    ];

    // Idle goes first so a standing wobble never counts as climbing.
    public static readonly IReadOnlyList<IPaceFilterConfig> PaceFilters =
    [
        new IdleFilter.Config(0.5),
        new GradeFilter.Config(2),
        new SpeedSegmentFilter.Config(100),
    ];

    // The comparison baseline: every optional metric on, with thresholds at their most permissive.
    public static readonly IReadOnlyList<IPaceFilterConfig> UnfilteredPaceFilters =
    [
        new IdleFilter.Config(0),
        new GradeFilter.Config(0),
        new SpeedSegmentFilter.Config(100),
    ];

    public static readonly IReadOnlyList<ITerrainFilterConfig> UnfilteredTerrainFilters =
    [
        new SteepestSegmentFilter.Config(100),
        new LongestClimbFilter.Config(0),
    ];

    // Ascent and descent are plain sums until ElevationGainFilter is added to ignore GPS wobble.
    public static readonly IReadOnlyList<ITerrainFilterConfig> TerrainFilters =
    [
        new SteepestSegmentFilter.Config(100),
        new LongestClimbFilter.Config(),
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
