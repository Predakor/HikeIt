using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Analytics.Route.Filters;

// The state the route analytic filters fill in. A metric stays at its default when its filter is not configured.
public sealed class RouteAnalyticContext(IReadOnlyList<GpxPoint> points, IReadOnlyList<GpxGain> gains)
{
    public IReadOnlyList<GpxPoint> Points { get; } = points;
    public IReadOnlyList<GpxGain> Gains { get; } = gains;

    public double TotalDistanceMeters { get; set; }
    public double TotalAscentMeters { get; set; }
    public double TotalDescentMeters { get; set; }

    public double HighestElevationMeters { get; set; }
    public double LowestElevationMeters { get; set; }

    public float AverageSlopePercent { get; set; }
    public float AverageAscentSlopePercent { get; set; }
    public float AverageDescentSlopePercent { get; set; }

    public double? SteepestClimbPercent { get; set; }
    public double? SteepestDescentPercent { get; set; }

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
        };
    }
}
