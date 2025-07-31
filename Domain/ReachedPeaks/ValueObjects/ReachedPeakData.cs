using Domain.Trips.ValueObjects;

namespace Domain.ReachedPeaks.ValueObjects;

public sealed record ReachedPeakData {
    public required Guid Id { get; init; }
    public required GpxPoint Location { get; init; }
    public required int PeakId { get; init; }
    public required int RegionID { get; init; }
    public required bool FirstTime { get; init; }
    public required DateTime? TimeReached { get; init; }
    public required uint ReachedAtDistanceMeters { get; init; }
}
