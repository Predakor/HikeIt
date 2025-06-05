using Domain.Entiites.Peaks;

namespace Domain.Trips.ValueObjects.TripAnalytics;

public class ReachedPeak {
    public GpxPoint GpxPoint { get; set; }
    public DateTime? TimeReached { get; set; }
    public int? PeakId { get; set; }
    public Peak? Peak { get; set; }
}
