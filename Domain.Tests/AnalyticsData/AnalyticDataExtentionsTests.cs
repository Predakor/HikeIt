using Domain.TripAnalytics.Commands;
using Domain.Trips.ValueObjects;

namespace Domain.Tests.AnalyticsData;

public class AnalyticsProcesing {
    static readonly int distanceTreshold = 10;

    static GpxPoint Pt(double ele) => new(0, 0, ele);

    [Fact]
    public void ReturnsOriginal_WhenLessThanTwoPeaks() {
        var input = new List<GpxPoint> { Pt(100) };
        var result = input.MergeNearbyPeaksByElevation(distanceTreshold);

        Assert.Single(result);
        Assert.Equal(100, result[0].Ele);
    }

    [Fact]
    public void MergesTwoClosePeaks() {
        var input = new List<GpxPoint> { Pt(100), Pt(101), Pt(102) };

        var result = input.MergeNearbyPeaksByElevation(distanceTreshold);
        Assert.Single(result);
        Assert.Equal(102, result[0].Ele);
    }

    [Fact]
    public void SeparatesDistantPeaks() {
        var input = new List<GpxPoint> { Pt(100), Pt(130), Pt(100) };
        var result = input.MergeNearbyPeaksByElevation(distanceTreshold);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void HandlesMultipleWindows() {
        var input = new List<GpxPoint>
        {
            Pt(100),
            Pt(101),
            Pt(102), // small window
            Pt(200),
            Pt(201),
            Pt(202), // another small window
        };
        var result = input.MergeNearbyPeaksByElevation(distanceTreshold);
        Assert.Equal(2, result.Count);
        Assert.Equal(102, result[0].Ele); // center of window 0–2
        Assert.Equal(202, result[1].Ele); // center of window 3–5
    }

    [Fact]
    public void HandlesUnclosedFinalWindow() {
        var input = new List<GpxPoint> { Pt(100), Pt(101), Pt(102), Pt(103) };
        var result = input.MergeNearbyPeaksByElevation(distanceTreshold * 2);

        Assert.Single(result);
        Assert.Equal(103, result[0].Ele);
    }
}
