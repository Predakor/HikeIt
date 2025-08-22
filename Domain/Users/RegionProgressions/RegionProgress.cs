using Domain.Common.Abstractions;
using Domain.Locations.Regions;
using Domain.Users.Root;

namespace Domain.Users.RegionProgressions;

public class RegionProgress : IEntity<Guid> {
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public int RegionId { get; set; }

    public short TotalPeaksInRegion { get; init; }
    public short TotalReachedPeaks { get; set; }
    public short UniqueReachedPeaks { get; set; }

    //nav properties
    public User User { get; set; } = default!;
    public Region Region { get; set; } = default!;

    public Dictionary<int, short> PeakVisits { get; private set; } = [];

    public static RegionProgress Create(Guid userId, int RegionId, short totalPeaks) {
        return new() {
            Id = Guid.NewGuid(),
            UserId = userId,
            RegionId = RegionId,
            TotalPeaksInRegion = totalPeaks,
        };
    }

    public RegionProgress AddPeakVisits(IEnumerable<int> peaksIds) {
        foreach (var peakId in peaksIds) {
            TotalReachedPeaks++;
            if (PeakVisits.ContainsKey(peakId)) {
                PeakVisits[peakId] += 1;
                continue;
            }

            UniqueReachedPeaks++;
            PeakVisits.Add(peakId, 1);
        }
        return this;
    }

    public RegionProgress RemovePeakVisits(IEnumerable<int> peaksIds) {
        foreach (var peakId in peaksIds) {
            if (!PeakVisits.ContainsKey(peakId)) {
                continue;
            }

            TotalReachedPeaks = TotalReachedPeaks.Decrement();

            var visits = PeakVisits[peakId];
            if (visits == 1) {
                PeakVisits.Remove(peakId);
                UniqueReachedPeaks = UniqueReachedPeaks.Decrement();
                continue;
            }

            PeakVisits[peakId] = visits.Decrement();
        }
        return this;
    }
}

static class Extentions {
    public static short Decrement(this short value, int decrement = 1) {
        return (short)Math.Max(value - decrement, 0);
    }
}
