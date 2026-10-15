using System.Text.Json;
using Application.Commons.Abstractions;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests.Pipeline;

public class CoordinateSmoothingFilterTests
{
    static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private const double StraightLat = 50;

    [Fact]
    public void Apply_NoisyStraightTrack_ReducesDeviationFromLine()
    {
        // Straight east-west line at Lat 50 with alternating GPS noise. Lon changes along the track,
        // so the lag an EMA adds along the track does not change the perpendicular deviation.
        var points = NoisyStraightTrack(200);
        var deviationBefore = TotalDeviation(points);

        new CoordinateSmoothingFilter(new CoordinateSmoothingFilter.Config(0.5)).Apply(points);

        Assert.True(TotalDeviation(points) < deviationBefore);
    }

    [Fact]
    public void Apply_LeavesElevationTimeAndPinnedUnchanged()
    {
        var points = Track(3);
        points[1].Ele = 120;
        points[1].Time = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        points[2].Pinned = true;
        var elevations = points.Select(p => p.Ele).ToList();
        var times = points.Select(p => p.Time).ToList();
        var pinned = points.Select(p => p.Pinned).ToList();

        new CoordinateSmoothingFilter(new CoordinateSmoothingFilter.Config(0.5)).Apply(points);

        Assert.Equal(elevations, points.Select(p => p.Ele).ToList());
        Assert.Equal(times, points.Select(p => p.Time).ToList());
        Assert.Equal(pinned, points.Select(p => p.Pinned).ToList());
    }

    [Fact]
    public void Apply_AlphaOne_LeavesCoordinatesUnchanged()
    {
        var points = NoisyStraightTrack(20);
        var lats = points.Select(p => p.Lat).ToList();
        var lons = points.Select(p => p.Lon).ToList();

        new CoordinateSmoothingFilter(new CoordinateSmoothingFilter.Config(1)).Apply(points);

        Assert.Equal(lats, points.Select(p => p.Lat).ToList());
        Assert.Equal(lons, points.Select(p => p.Lon).ToList());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    public void Constructor_AlphaOutsideZeroToOne_Throws(double alpha)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new CoordinateSmoothingFilter(new CoordinateSmoothingFilter.Config(alpha)));
    }

    [Fact]
    public void Json_Deserialises_AsPointFilterConfig_AndFactoryBuildsFilter()
    {
        var config = JsonSerializer.Deserialize<IPointFilterConfig>(
            """{"FilterType":"CoordinateSmoothingFilter","value":{"Alpha":0.5}}""",
            Json
        );

        Assert.IsType<CoordinateSmoothingFilter.Config>(config);
        Assert.Equal(0.5, ((CoordinateSmoothingFilter.Config)config!).Value.Alpha);
        Assert.IsType<CoordinateSmoothingFilter>(RouteVisualizationService.GpxFilterFactory.CreateFilter(config!));
    }

    [Fact]
    public void CleanupDefaults_DoNotContainCoordinateSmoothing()
    {
        Assert.DoesNotContain(PipelineDefaults.CleanupFilters, config => config is CoordinateSmoothingFilter.Config);
    }

    static double TotalDeviation(IEnumerable<MutableGpxPoint> points) =>
        points.Sum(p => Math.Abs(p.Lat - StraightLat));

    // Lat alternates +-0.00005 around the straight line, Lon steps evenly along it.
    static List<MutableGpxPoint> NoisyStraightTrack(int count) =>
        [.. Enumerable.Range(0, count).Select(i => new MutableGpxPoint(
            StraightLat + (i % 2 == 0 ? 0.00005 : -0.00005),
            20 + i * 0.0001,
            100
        ))];

    static List<MutableGpxPoint> Track(int count) =>
        [.. Enumerable.Range(0, count).Select(i => new MutableGpxPoint(StraightLat, 20 + i * 0.0001, 100 + i))];
}
