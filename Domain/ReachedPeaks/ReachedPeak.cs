using Domain.Common.Result;
using Domain.Interfaces;
using Domain.Mountains.Peaks;
using Domain.ReachedPeaks.Rules;
using Domain.Trips;
using Domain.Users;
using Domain.Users.Extentions;

namespace Domain.ReachedPeaks;

public class ReachedPeak : IEntity<Guid> {
    public Guid Id { get; init; }
    public bool FirstTime { get; private set; }
    public DateTime? ReachedAtTime { get; private set; }
    public uint? ReachedAtDistanceMeters { get; private set; }

    // Foreign Keys
    public required Guid TripId { get; init; }
    public required Guid UserId { get; init; }
    public required int PeakId { get; init; }

    // Navigation
    public Trip Trip { get; set; }
    public User User { get; set; }
    public Peak Peak { get; set; }

    public static ReachedPeak Create(Peak peak, Trip trip, User user, DateTime? reachTime = null) {
        return Create(peak.Id, trip.Id, user.Id, reachTime);
    }

    public static ReachedPeak Create(
        int PeakId,
        Guid TripId,
        Guid UserId,
        DateTime? reachTime = null,
        bool firstTime = false
    ) {
        var reachedPeak = new ReachedPeak {
            Id = Guid.NewGuid(),
            TripId = TripId,
            UserId = UserId,
            PeakId = PeakId,
            FirstTime = firstTime,
        };

        if (reachTime.HasValue) {
            reachedPeak.AddReachTime(reachTime.Value);
        }

        return reachedPeak;
    }

    public Result<ReachedPeak> MarkAsFirstTime(bool isFirstTime) {
        FirstTime = isFirstTime;
        return this;
    }

    public Result<ReachedPeak> AddReachTime(DateTime time) {
        var rule = new DateNotInTheFuture(time);
        return rule.Check()
            .Map(_ => {
                ReachedAtTime = time.ToUniversalTime();
                return this;
            });
    }

    public Result<ReachedPeak> AddReachedDistance(int distance) {
        ReachedAtDistanceMeters = distance.ToSafeUint();
        return this;
    }
}
