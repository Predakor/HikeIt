namespace Domain.Trips.ValueObjects.TripAnalytics;

//Owned Type
public class TripAnalytic {
    public double TotalDistanceKm { get; init; }
    public double TotalAscent { get; init; }
    public double TotalDescent { get; init; }
    public double MinElevation { get; init; }
    public double MaxElevation { get; init; }

    //Owned Types
    public TripTimeAnalytic? TimeAnalytics { get; set; }
    public List<ReachedPeak>? ReachedPeaks { get; set; }
}
