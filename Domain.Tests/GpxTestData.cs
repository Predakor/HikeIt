using Domain.Trips.ValueObjects;

namespace Domain.Tests;

public static class GpxTestData {
    public static readonly TripAnalyticData JitteryTripMockup = new(
        [
            new(100, 120, 12),
            new(100, 120, 12.5),
            new(100, 220, 13.1), // jitter dip
            new(100, 120, 12.9),
            new(100, 120, 12.5),
            new(100, 120, 12.6),
            new(100, 220, 15), // jitter dip
            new(100, 120, 16),
            new(100, 120, 17),
            new(100, 120, 17),
            new(100, 220, 50), // jitter big dip
            new(100, 120, 20),
            new(100, 120, 20.1),
        ]
    );
    public static readonly TripAnalyticData DownUpPath = new(
        [
            new(100, 120, 100),
            new(100, 120, 99),
            new(100, 220, 98), // jitter dip
            new(100, 120, 97),
            new(100, 120, 97),
            new(100, 120, 97.3),
            new(100, 220, 98), // jitter dip
            new(100, 120, 94),
            new(100, 120, 95),
            new(100, 120, 100),
            new(100, 220, 101), // jitter big dip
            new(100, 120, 102),
            new(100, 120, 103),
        ]
    );
    public static TheoryData<TripAnalyticData> AllTripData { get; } =
        [JitteryTripMockup, DownUpPath];
}
