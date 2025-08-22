using Domain.Common.Abstractions;
using Domain.ReachedPeaks;

namespace Domain.Trips.Analytics.Peaks;

public class PeaksAnalytic : IEntity<Guid> {
    public Guid Id { get; init; }

    public uint Total { get; private set; }
    public uint Unique { get; private set; }
    public uint New { get; private set; }

    public static PeaksAnalytic Create(Guid id, IEnumerable<ReachedPeak> peaks) {
        if (!peaks.Any()) {
            throw new Exception("Peaks are empty");
        }

        return new() {
            Id = id,
            Total = peaks.GetCount(),
            Unique = peaks.DistinctBy(p => p.PeakId).GetCount(),
            New = peaks.Where(p => p.FirstTime).GetCount(),
        };
    }
}

static class Extentions {
    public static uint GetCount<T>(this IEnumerable<T> items) {
        return (uint)Math.Clamp(items.Count(), 0, uint.MaxValue);
    }
}
