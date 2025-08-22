using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests;

public class GpxDataBuilderTest {
    [Fact]
    public void TripAnalyticsBuilder_Should_ReturnBuilder() {
        var builder = new GpxDataBuilder(GpxTestData.DownUpPath.Points);

        Assert.NotNull(builder);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void TripAnalyticsBuilder_Should_DampenSpikes(AnalyticData data) {
        var points = new GpxDataBuilder(data.Points).ApplyMedianFilter(5).Build();

        double MaxElevationJump(List<GpxPoint> points) {
            double maxJump = 0;
            for (int i = 1; i < points.Count; i++) {
                double jump = Math.Abs(points[i].Ele - points[i - 1].Ele);
                if (jump > maxJump)
                    maxJump = jump;
            }
            return maxJump;
        }

        var biggestSpike = MaxElevationJump(points.Points);

        Assert.True(biggestSpike < 5);
    }
}
