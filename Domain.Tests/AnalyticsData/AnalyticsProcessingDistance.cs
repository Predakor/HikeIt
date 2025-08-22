using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Domain.Tests.AnalyticsData;

public class AnalyticsProcessingDistance {
    static readonly int distanceThreshold = 10;

    static GpxPoint Pt(double lat, double lon) => new(lat, lon, 0);

    [Fact]
    public void ReturnsOriginal_WhenLessThanTwoPeaks() {
        var input = new List<GpxPoint> { Pt(0, 0) };
        var result = input.MergeNearbyPeakByDistance(distanceThreshold);

        Assert.Single(result);
        Assert.Equal(0, result[0].Lat);
        Assert.Equal(0, result[0].Lon);
    }

    [Fact]
    public void MergesTwoClosePeaks() {
        var input = new List<GpxPoint>
        {
            Pt(0, 0.00001), // very close, should merge
            Pt(0, 0.00002),
        };

        var result = input.MergeNearbyPeakByDistance(distanceThreshold);

        Assert.Single(result);
    }

    [Fact]
    public void DontMergesTwoDistantPeaks() {
        var input = new List<GpxPoint>
        {
            Pt(0, 0),
            Pt(0, 0.0002), // ≈22 meters apart from the first
            Pt(0, 0.0004), // ≈22 meters apart from the second
        };
        var result = input.MergeNearbyPeakByDistance(distanceThreshold);

        // Since all points are farther apart than threshold, all should be kept
        Assert.Equal(3, result.Count);
    }
}
