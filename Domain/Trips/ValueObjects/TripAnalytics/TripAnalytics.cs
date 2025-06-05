namespace Domain.Trips.ValueObjects.TripAnalytics;

//Owned Type
public class TripAnalytic {
    public double TotalDistanceKm { get; init; }
    public double TotalAscent { get; init; }
    public double TotalDescent { get; init; }
    public double MinElevation { get; init; }
    public double MaxElevation { get; init; }

    //Owned Type
    public TripTimeAnalytic? TimeAnalytics { get; set; }

}
