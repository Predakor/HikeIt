using Domain.ReachedPeaks;

namespace Domain.TripAnalytics.ValueObjects.PeaksAnalytics;

//Owned type
public class PeaksAnalytic {
    ICollection<ReachedPeak> Peaks { get; set; }
}
