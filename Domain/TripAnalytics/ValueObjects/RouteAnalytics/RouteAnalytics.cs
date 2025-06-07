namespace Domain.TripAnalytics.ValueObjects.RouteAnalytics;

//Owned Type
public class RouteAnalytic {
    public required double TotalDistanceKm { get; init; }
    public required double TotalAscent { get; init; }
    public required double TotalDescent { get; init; }

    public required double HighestElevation { get; init; }
    public required double LowestElevation { get; init; }

    public required short AverageSlope { get; init; }
    public required short AverageAscentSlope { get; init; }
    public required short AverageDescentSlope { get; init; }

}
