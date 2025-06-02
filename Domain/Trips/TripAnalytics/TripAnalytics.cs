namespace Domain.Trips.TripAnalytics;

//Owned Type
public class TripAnalytic {
    public double TotalDistanceKm { get; set; }
    public double TotalAscent { get; set; }
    public double TotalDescent { get; set; }
    public double MinElevation { get; set; }
    public double MaxElevation { get; set; }

    //Owned Type
    public TripTimeAnalytic? TimeAnalytics { get; set; }
}
