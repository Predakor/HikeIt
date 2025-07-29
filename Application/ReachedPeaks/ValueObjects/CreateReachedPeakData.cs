using Domain.Trips.ValueObjects;

namespace Application.ReachedPeaks.ValueObjects;

public class CreateReachedPeakData {
    public readonly Guid Id;
    public readonly GpxPoint Location;

    CreateReachedPeakData(GpxPoint point) {
        Id = Guid.NewGuid();
        Location = point;

        if (point.Time is not null) {
            TimeReached = point.Time;
        }
    }

    public static CreateReachedPeakData FromGpxPoint(GpxPointWithDistance point) {
        return new(point.BasePoint) { ReachedAtDistanceMeters = (uint)point.DistanceFromStart };
    }

    public int PeakId;

    public bool FirstTime;
    public DateTime? TimeReached;
    public uint ReachedAtDistanceMeters;
}
