namespace Domain.TrackAnalytics;

//Owned Type
public class TrackAnalytic {
    public double TotalDistanceKm { get; set; }
    public double TotalAscent { get; set; }
    public double TotalDescent { get; set; }
    public double MinElevation { get; set; }
    public double MaxElevation { get; set; }

    //Owned Type
    public TrackTimeAnalytic? TimeAnalytics { get; set; }
}
