using Domain.Common;
using Domain.Common.Result;
using Domain.Interfaces;
using Domain.Mountains.Peaks;
using Domain.Trips;
using Domain.Users;

namespace Domain.ReachedPeaks;

public class ReachedPeak : IEntity<Guid> {
    public Guid Id { get; init; }
    public bool FirstTime { get; private set; }
    public DateTime? TimeReached { get; private set; }

    // Foreign Keys
    public required Guid TripId { get; init; }
    public required Guid UserId { get; init; }
    public required int PeakId { get; init; }

    // Navigation
    public Trip? Trip { get; set; }
    public User? User { get; set; }
    public Peak? Peak { get; set; }

    public static ReachedPeak Create(
        int PeakId,
        Guid TripId,
        Guid UserId,
        DateTime? reachTime = null
    ) {
        var reachedPeak = new ReachedPeak {
            Id = Guid.NewGuid(),
            TripId = TripId,
            UserId = UserId,
            PeakId = PeakId,
        };

        if (reachTime.HasValue) {
            reachedPeak.AddReachTime(reachTime.Value);
        }

        return reachedPeak;
    }

    public static ReachedPeak Create(Peak peak, Trip trip, User user, DateTime? reachTime = null) {
        var reachedPeak = new ReachedPeak() {
            Id = Guid.NewGuid(),
            PeakId = peak.Id,
            TripId = trip.Id,
            UserId = user.Id,
            Peak = peak,
            Trip = trip,
            User = user,
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
        var rule = new TimeMustBeSmallerThanToday(time);
        return rule.Check()
            .Match<Result<ReachedPeak>>(
                ok => {
                    TimeReached = time.ToUniversalTime();
                    return this;
                },
                error => error
            );
    }

    class TimeMustBeSmallerThanToday(DateTime time) : IRule {
        public string Name => "Invalid Time";
        public string Message => "Time is set to a future date please enter correct date";

        public Result<bool> Check() {
            if (time <= DateTime.UtcNow) {
                return true;
            }
            return Errors.RuleViolation(this);
        }
    }
}
