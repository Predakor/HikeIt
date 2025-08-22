namespace Domain.Trips.Analytics.Route;

//Owned Type
public class RouteAnalytic {
    public required double TotalDistanceMeters { get; init; }
    public required double TotalAscentMeters { get; init; }
    public required double TotalDescentMeters { get; init; }

    public required double HighestElevationMeters { get; init; }
    public required double LowestElevationMeters { get; init; }

    public required float AverageSlopePercent { get; init; }
    public required float AverageAscentSlopePercent { get; init; }
    public required float AverageDescentSlopePercent { get; init; }

}
