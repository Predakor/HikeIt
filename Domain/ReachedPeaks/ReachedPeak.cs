using Domain.Entiites.Peaks;
using Domain.Entiites.Users;
using Domain.Trips;

namespace Domain.ReachedPeaks;

public class ReachedPeak : IEntity<Guid> {
    public Guid Id { get; init; }
    public DateTime? TimeReached { get; set; }

    // Foreign Keys
    public required Guid TripId { get; set; }
    public required Guid UserId { get; set; }
    public required int PeakId { get; set; }

    // Navigation
    public Trip? Trip { get; set; }
    public User? User { get; set; }
    public Peak? Peak { get; set; }

    public static ReachedPeak Create(Guid TripId, Guid UserId, int PeakId, DateTime? reachTime = null) {
        return new ReachedPeak {
            Id = new Guid(),
            TripId = TripId,
            UserId = UserId,
            PeakId = PeakId,
            TimeReached = reachTime
        };
    }
}
