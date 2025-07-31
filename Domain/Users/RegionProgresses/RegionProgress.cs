using Domain.Interfaces;
using Domain.Mountains.Regions;

namespace Domain.Users.RegionProgresses;

public class RegionProgress : IEntity<Guid> {
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public int RegionId { get; set; }

    public short TotalPeaksInRegion { get; init; }
    public short TotalReachedPeaks { get; set; }
    public short UniqueReachedPeaks { get; set; }

    //nav properties
    public User User { get; set; } = default!;
    public Region Region { get; set; }


    public Dictionary<int, short> PeakVisits { get; private set; } = [];

    public static RegionProgress Create(Guid userId, int RegionId, short totalPeaks) {
        return new() {
            Id = Guid.NewGuid(),
            UserId = userId,
            RegionId = RegionId,
            TotalPeaksInRegion = totalPeaks
        };
    }

    public RegionProgress AddPeakVisits(IEnumerable<int> peaksIds) {
        foreach (var peakId in peaksIds) {
            if (PeakVisits.ContainsKey(peakId)) {
                PeakVisits[peakId] += 1;
                continue;
            }
            PeakVisits.Add(peakId, 1);
        }
        return this;
    }

    public RegionProgress RemovePeakVisits(IEnumerable<int> peaksIds) {
        foreach (var peakId in peaksIds) {
            if (!PeakVisits.ContainsKey(peakId)) {
                continue;
            }

            var visits = PeakVisits[peakId];
            if (visits == 1) {
                PeakVisits.Remove(peakId);
                continue;
            }

            PeakVisits[peakId] = (short)Math.Max(visits - 1, 0);
        }
        return this;
    }
}
