using Domain.Common.Geography;
using Domain.Common.Geography.ValueObjects;

namespace Domain.Tests.Pipeline;

public class GeoBoundsTests
{
    private const double MetersPerDegreeLat = 111_320;

    [Fact]
    public void Around_SinglePoint_PadsByTheMarginOnEverySide()
    {
        var bounds = GeoBounds.Around([new GpxPoint(50, 20, 0)], marginM: 100);

        Assert.True(bounds.MinLat < 50 && bounds.MaxLat > 50);
        Assert.True(bounds.MinLon < 20 && bounds.MaxLon > 20);
        Assert.True((50 - bounds.MinLat) * MetersPerDegreeLat >= 100);
        Assert.True((bounds.MaxLat - 50) * MetersPerDegreeLat >= 100);
    }

    [Fact]
    public void Around_PointsFarApart_CoverEveryPoint()
    {
        GpxPoint[] points = [new(49.20, 19.90, 0), new(49.70, 20.50, 0), new(49.45, 20.10, 0)];

        var bounds = GeoBounds.Around(points, marginM: 100);

        foreach (var point in points)
        {
            Assert.InRange(point.Lat, bounds.MinLat, bounds.MaxLat);
            Assert.InRange(point.Lon, bounds.MinLon, bounds.MaxLon);
        }
    }

    [Fact]
    public void Around_LongitudePadding_GrowsWithLatitude()
    {
        var equator = GeoBounds.Around([new GpxPoint(0, 20, 0)], marginM: 1_000);
        var north = GeoBounds.Around([new GpxPoint(60, 20, 0)], marginM: 1_000);

        Assert.True(north.MaxLon - north.MinLon > equator.MaxLon - equator.MinLon);
    }

    [Fact]
    public void Around_NoPoints_Throws()
    {
        Assert.Throws<ArgumentException>(() => GeoBounds.Around([], marginM: 100));
    }
}
