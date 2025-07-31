using Domain.Interfaces;
using Domain.Mountains.Regions;

namespace Domain.Users.RegionProgres;

public class RegionProgress : IEntity<Guid> {
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public int RegionId { get; init; }

    public short TotalPeaks { get; init; }
    public short ReachedPeaks { get; private set; }

    //nav properties
    public User User { get; private set; }
    public Region Region { get; private set; }


    public Dictionary<int, short> PeakVisits { get; private set; } = [];

    public static RegionProgress Create(Guid userId, int RegionId, short totalPeaks) {
        return new() {
            Id = Guid.NewGuid(),
            UserId = userId,
            RegionId = RegionId,
            TotalPeaks = totalPeaks
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
