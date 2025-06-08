using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.ValueObjects;

namespace Domain.Tests;

public class GpxDataBuilderTest {

    [Fact]
    public void TripAnalyticsBuilder_Should_ReturnBuilder() {
        var builder = new GpxDataBuilder(GpxTestData.DownUpPath);

        Assert.NotNull(builder);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void TripAnalyticsBuilder_Should_DampenSpikes(TripAnalyticData data) {
        var points = new GpxDataBuilder(data).ApplyMedianFilter(5).Build();

        double MaxElevationJump(List<GpxPoint> points) {
            double maxJump = 0;
            for (int i = 1; i < points.Count; i++) {
                double jump = Math.Abs(points[i].Ele - points[i - 1].Ele);
                if (jump > maxJump)
                    maxJump = jump;
            }
            return maxJump;
        }

        var biggestSpike = MaxElevationJump(points.Data);

        Assert.True(biggestSpike < 5);
    }

    [Theory]
    [MemberData(nameof(GpxTestData.AllTripData), MemberType = typeof(GpxTestData))]
    public void TripAnalyticsBuilder_FlattenMicroJumps_Should_FlattenSmallJitters(
        TripAnalyticData data
    ) {
        const float jitterThreshold = 0.5f; // use same threshold as in method

        var smoothedPoints = new GpxDataBuilder(data)
            .ApplyEmaSmoothing(jitterThreshold)
            .Build()
            .Data;

        for (int i = 1; i < smoothedPoints.Count; i++) {
            double diff = Math.Abs(smoothedPoints[i].Ele - smoothedPoints[i - 1].Ele);

            Assert.True(
                diff >= jitterThreshold
                    || diff == 0
                    || IsMeaningfulChange(smoothedPoints, i, jitterThreshold),
                $"Elevation jump {diff} at index {i} should be >= {jitterThreshold} or flattened."
            );
        }
    }

    private bool IsMeaningfulChange(List<GpxPoint> points, int idx, double threshold) {
        if (idx + 1 < points.Count) {
            double overallChange = Math.Abs(points[idx + 1].Ele - points[idx - 1].Ele);
            return overallChange > threshold;
        }
        return false;
    }
}
