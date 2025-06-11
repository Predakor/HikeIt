using Domain.Trips.ValueObjects;

namespace Domain.Tests;

public static class GpxTestData {
    public static readonly AnalyticData JitteryTripMockup = new(
        [
            new(50.0000, 19.0000, 12),
            new(50.0001, 19.0002, 12.5),
            new(50.0002, 19.0004, 13.1), // jitter dip
            new(50.0003, 19.0006, 12.9),
            new(50.0004, 19.0008, 12.5),
            new(50.0005, 19.0010, 12.6),
            new(50.0006, 19.0012, 15), // jitter dip
            new(50.0007, 19.0014, 16),
            new(50.0008, 19.0016, 17),
            new(50.0009, 19.0018, 17),
            new(50.0010, 19.0020, 50), // jitter big dip
            new(50.0011, 19.0022, 20),
            new(50.0012, 19.0024, 20.1),
        ]
    );

    public static readonly AnalyticData DownUpPath = new(
        [
            new(50.1000, 19.1000, 100),
            new(50.1001, 19.1002, 99),
            new(50.1002, 19.1004, 98), // jitter dip
            new(50.1003, 19.1006, 97),
            new(50.1004, 19.1008, 97),
            new(50.1005, 19.1010, 97.3),
            new(50.1006, 19.1012, 98), // jitter dip
            new(50.1007, 19.1014, 94),
            new(50.1008, 19.1016, 95),
            new(50.1009, 19.1018, 100),
            new(50.1010, 19.1020, 101), // jitter big dip
            new(50.1011, 19.1022, 102),
            new(50.1012, 19.1024, 103),
        ]
    );

    public static TheoryData<AnalyticData> AllTripData { get; } =
        [JitteryTripMockup, DownUpPath];
}
