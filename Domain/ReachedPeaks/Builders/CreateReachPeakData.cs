using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;
using Domain.ReachedPeaks.ValueObjects;

namespace Domain.ReachedPeaks.Builders;

public class ReachedPeakDataBuilder {
    public readonly Guid Id;
    public readonly GpxPoint Location;

    public int PeakId { get; private set; }
    public int RegionID { get; private set; }
    public bool FirstTime { get; private set; }
    public DateTime? TimeReached { get; private set; }
    public uint ReachedAtDistanceMeters { get; private set; }

    ReachedPeakDataBuilder(GpxPoint location) {
        Id = Guid.NewGuid();
        Location = location;
        WithTime(location.Time);
    }

    public static ReachedPeakDataBuilder Create(GpxPoint location) {
        return new(location);
    }

    public ReachedPeakDataBuilder WithPeak(Peak peak) {
        PeakId = peak.Id;
        RegionID = peak.RegionID;
        return this;
    }

    public ReachedPeakDataBuilder SetFirstTimeReached(bool reachedFirstTime) {
        FirstTime = reachedFirstTime;
        return this;
    }

    public ReachedPeakDataBuilder WithTime(DateTime? time) {
        if (time is not null) {
            TimeReached = time;
        }

        return this;
    }

    public ReachedPeakDataBuilder WithDistanceFromStart(float reachedAtDistance) {
        ReachedAtDistanceMeters = (uint)reachedAtDistance;
        return this;
    }

    public CreateReachedPeak Build() {
        return new CreateReachedPeak() {
            Id = Id,
            Location = Location,
            PeakId = PeakId,
            RegionID = RegionID,
            ReachedAtDistanceMeters = ReachedAtDistanceMeters,
            TimeReached = TimeReached,
            FirstTime = FirstTime,
        };
    }
}
