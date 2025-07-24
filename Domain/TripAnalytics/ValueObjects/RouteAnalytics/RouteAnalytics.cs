namespace Domain.TripAnalytics.ValueObjects.RouteAnalytics;

//Owned Type
public class RouteAnalytic {

    //Ideas
    //avg elevation gain per m
    //total ascent/acerageAscentSlope
    // same for descend
    // and ascent


    /// <summary>
    /// Actually its in meters 
    /// need to change the name but db confis ;c
    /// </summary>
    public required double TotalDistanceKm { get; init; }
    public required double TotalAscent { get; init; }
    public required double TotalDescent { get; init; }

    public required double HighestElevation { get; init; }
    public required double LowestElevation { get; init; }

    public required float AverageSlope { get; init; }
    public required float AverageAscentSlope { get; init; }
    public required float AverageDescentSlope { get; init; }

}
