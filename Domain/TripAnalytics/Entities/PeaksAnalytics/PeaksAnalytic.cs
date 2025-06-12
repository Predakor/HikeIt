using Domain.ReachedPeaks;

namespace Domain.TripAnalytics.Entities.PeaksAnalytics;

public class PeaksAnalytic : IEntity<Guid> {
    public Guid Id { get; init; }

    public required ICollection<ReachedPeak> ReachedPeaks { get; init; } = [];
    public ICollection<ReachedPeak>? NewPeaks { get; init; }

    //nav properties

    public static PeaksAnalytic Create(List<ReachedPeak> peaks, List<ReachedPeak>? newPeaks = null) {
        return new() {
            Id = Guid.NewGuid(),
            ReachedPeaks = peaks,
            NewPeaks = newPeaks,
        };
    }

}
