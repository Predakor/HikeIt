using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Analytics.Route.Filters;

// Plain metrics are computed up front; filters replace or add to them. Optional metrics stay null until their filter runs.
public sealed class RouteAnalyticContext(IReadOnlyList<GpxPoint> points, IReadOnlyList<GpxGain> gains)
{
    public IReadOnlyList<GpxPoint> Points { get; } = points;
    public IReadOnlyList<GpxGain> Gains { get; } = gains;

    public double TotalDistanceMeters { get; } = gains.Sum(gain => gain.DistanceDelta);

    // Plain sums of every rise and fall; ElevationGainFilter replaces them.
    public double TotalAscentMeters { get; set; } =
        gains
            .Where(gain => gain.ElevationDelta > 0)
            .Sum(gain => gain.ElevationDelta);
    public double TotalDescentMeters { get; set; } =
        Math.Abs(
            gains
                .Where(gain => gain.ElevationDelta < 0)
                .Sum(gain => gain.ElevationDelta)
        );

    public double HighestElevationMeters { get; } =
        points.Count == 0
            ? 0
            : points.Max(point => point.Ele);
    public double LowestElevationMeters { get; } =
        points.Count == 0
            ? 0
            : points.Min(point => point.Ele);

    public float AverageSlopePercent { get; } = AverageOf(gains, slope => true);
    public float AverageAscentSlopePercent { get; } = AverageOf(gains, slope => slope > 0);
    public float AverageDescentSlopePercent { get; } = AverageOf(gains, slope => slope < 0);

    public double? SteepestClimbPercent { get; set; }
    public double? SteepestDescentPercent { get; set; }

    public double? LongestClimbDistanceM { get; set; }
    public double? LongestClimbPercent { get; set; }
    public double? LongestDescentDistanceM { get; set; }
    public double? LongestDescentPercent { get; set; }

    // A track with no climbing (or no descending) has no such slopes to average.
    private static float AverageOf(IReadOnlyList<GpxGain> gains, Func<float, bool> isIncluded)
    {
        return gains
            .Select(gain => gain.Slope)
            .Where(isIncluded)
            .DefaultIfEmpty(0)
            .Average();
    }

    public RouteAnalytic ToAnalytic()
    {
        return new RouteAnalytic
        {
            TotalDistanceMeters = TotalDistanceMeters,
            TotalAscentMeters = TotalAscentMeters,
            TotalDescentMeters = TotalDescentMeters,

            HighestElevationMeters = HighestElevationMeters,
            LowestElevationMeters = LowestElevationMeters,

            AverageSlopePercent = AverageSlopePercent,
            AverageAscentSlopePercent = AverageAscentSlopePercent,
            AverageDescentSlopePercent = AverageDescentSlopePercent,

            SteepestClimbPercent = SteepestClimbPercent,
            SteepestDescentPercent = SteepestDescentPercent,

            LongestClimbDistanceM = LongestClimbDistanceM,
            LongestClimbPercent = LongestClimbPercent,
            LongestDescentDistanceM = LongestDescentDistanceM,
            LongestDescentPercent = LongestDescentPercent,
        };
    }
}
