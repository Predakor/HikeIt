namespace Domain.Trips.Analytics.Route;

//Owned Type
public sealed class RouteAnalytic
{
    public required double TotalDistanceMeters { get; init; }
    public required double TotalAscentMeters { get; init; }
    public required double TotalDescentMeters { get; init; }

    public required double HighestElevationMeters { get; init; }
    public required double LowestElevationMeters { get; init; }

    public required float AverageSlopePercent { get; init; }
    public required float AverageAscentSlopePercent { get; init; }
    public required float AverageDescentSlopePercent { get; init; }

    // Null when the steepest segment metric is off, the track is shorter than its window, or the trip predates it.
    public double? SteepestClimbPercent { get; init; }
    public double? SteepestDescentPercent { get; init; }

    // Null when the longest climb metric is off, the track has no such segment, or the trip predates it.
    public double? LongestClimbDistanceM { get; init; }
    public double? LongestClimbPercent { get; init; }
    public double? LongestDescentDistanceM { get; init; }
    public double? LongestDescentPercent { get; init; }
}
