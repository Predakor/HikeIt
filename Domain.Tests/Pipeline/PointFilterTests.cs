using Domain.Trips.Analytics.Shared;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests.Pipeline;

public class PointFilterTests
{
    private static List<MutableGpxPoint> Track(params double[] elevations) =>
        [.. elevations.Select((ele, i) => new MutableGpxPoint(50 + i * 0.0001, 20, ele))];

    private static List<double> Elevations(IEnumerable<MutableGpxPoint> points) => [.. points.Select(p => p.Ele)];

    [Fact]
    public void MaxSpike_ClampsEachStepToTheLimit()
    {
        var points = Track(100, 120, 90);

        new MaxSpikeFilter(new MaxSpikeFilter.Config(5)).Apply(points);

        Assert.Equal([100d, 105d, 100d], Elevations(points));
    }

    [Fact]
    public void MaxSpike_StepsWithinTheLimit_AreUnchanged()
    {
        var points = Track(100, 103, 101);

        new MaxSpikeFilter(new MaxSpikeFilter.Config(5)).Apply(points);

        Assert.Equal([100d, 103d, 101d], Elevations(points));
    }

    [Fact]
    public void MaxSpike_SinglePoint_IsReturnedAsIs()
    {
        var points = Track(100);

        var result = new MaxSpikeFilter(new MaxSpikeFilter.Config(5)).Apply(points);

        Assert.Equal([100d], Elevations(result));
    }

    [Fact]
    public void MedianSmoothing_RemovesAnIsolatedSpike()
    {
        var points = Track(10, 10, 10, 500, 10, 10, 10);

        new MedianSmoothingFilter(new MedianSmoothingFilter.Config(3)).Apply(points);

        Assert.All(points, point => Assert.Equal(10, point.Ele));
    }

    [Fact]
    public void MedianSmoothing_DefaultTargets_LeaveCoordinatesAlone()
    {
        var points = Track(10, 500, 10);
        var latitudes = points
            .Select(p => p.Lat)
            .ToList();

        new MedianSmoothingFilter(new MedianSmoothingFilter.Config(3)).Apply(points);

        Assert.Equal(
            latitudes,
            points
                .Select(p => p.Lat)
                .ToList()
        );
    }

    [Fact]
    public void MedianSmoothing_NoTargets_Throws()
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            new MedianSmoothingFilter(new MedianSmoothingFilter.Config(3, PointFields.None))
        );
    }

    [Fact]
    public void Rounding_RoundsElevationByDefault()
    {
        var points = new List<MutableGpxPoint> { new(50.123456, 20.654321, 100.456) };

        new RoundingPrecisionFilter(new RoundingPrecisionFilter.Config(1)).Apply(points);

        Assert.Equal(100.5, points[0].Ele);
        Assert.Equal(50.123456, points[0].Lat);
        Assert.Equal(20.654321, points[0].Lon);
    }

    [Fact]
    public void Rounding_CoordinateTargets_RoundLatAndLonOnly()
    {
        var points = new List<MutableGpxPoint> { new(50.123456, 20.654321, 100.456) };

        new RoundingPrecisionFilter(new RoundingPrecisionFilter.Config(2, PointFields.Coordinates)).Apply(points);

        Assert.Equal(50.12, points[0].Lat);
        Assert.Equal(20.65, points[0].Lon);
        Assert.Equal(100.456, points[0].Ele);
    }

    [Fact]
    public void Rounding_NoTargets_Throws()
    {
        Assert.ThrowsAny<ArgumentException>(() =>
            new RoundingPrecisionFilter(new RoundingPrecisionFilter.Config(1, PointFields.None))
        );
    }

    [Fact]
    public void PinLocalMaxima_Disabled_PinsNothing()
    {
        var points = Track(100, 120, 110, 130, 90);

        new PinLocalMaximaFilter(new PinLocalMaximaFilter.Config(false)).Apply(points);

        Assert.DoesNotContain(points, point => point.Pinned);
    }

    [Fact]
    public void PinLocalMaxima_SinglePoint_PinsIt()
    {
        var points = Track(100);

        new PinLocalMaximaFilter(new PinLocalMaximaFilter.Config(true)).Apply(points);

        Assert.True(points[0].Pinned);
    }

    [Fact]
    public void Configs_BuildTheirOwnFilters()
    {
        Assert.IsType<MaxSpikeFilter>(new MaxSpikeFilter.Config(5).CreateFilter());
        Assert.IsType<MedianSmoothingFilter>(new MedianSmoothingFilter.Config(3).CreateFilter());
        Assert.IsType<RoundingPrecisionFilter>(new RoundingPrecisionFilter.Config(1).CreateFilter());
        Assert.IsType<PinLocalMaximaFilter>(new PinLocalMaximaFilter.Config(true).CreateFilter());
        Assert.IsType<SimplifyFilter>(new SimplifyFilter.Config(3).CreateFilter());
    }
}
