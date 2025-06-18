using Domain.Entiites.Peaks;
using Domain.Entiites.Users;
using Domain.Interfaces;
using Domain.Trips;

namespace Domain.ReachedPeaks;

public class ReachedPeak : IEntity<Guid> {
    public Guid Id { get; init; }
    public DateTime? TimeReached { get; set; }

    // Foreign Keys
    public required Guid TripId { get; init; }
    public required Guid UserId { get; init; }
    public required int PeakId { get; init; }

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
    public static ReachedPeak Create(Peak peak, Trip trip, User user, DateTime? reachTime = null) {
        return new ReachedPeak {
            Id = new Guid(),
            PeakId = peak.Id,
            TripId = trip.Id,
            UserId = user.Id,
            Peak = peak,
            Trip = trip,
            User = user,
            TimeReached = reachTime
        };
    }

}
