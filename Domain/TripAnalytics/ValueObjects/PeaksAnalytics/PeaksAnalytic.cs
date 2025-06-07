using Domain.ReachedPeaks;

namespace Domain.TripAnalytics.ValueObjects.PeaksAnalytics;

//Owned type
public class PeaksAnalytic {
    public required ICollection<ReachedPeak> Peaks { get; init; }
    public required ReachedPeak Highest { get; init; }
    public ICollection<ReachedPeak>? NewPeaks { get; init; }
}
