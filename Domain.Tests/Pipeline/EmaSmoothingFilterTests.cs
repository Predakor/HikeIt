using System.Text.Json;
using Application.Commons.Abstractions;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests.Pipeline;

public class EmaSmoothingFilterTests
{
    static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    private const double StraightLat = 50;

    [Fact]
    public void DefaultTargets_LeavesLatLonUnchanged_AndSmoothesElevation()
    {
        var points = NoisyStraightTrack(50);
        var lats = points.Select(p => p.Lat).ToList();
        var lons = points.Select(p => p.Lon).ToList();
        var elevationsBefore = points.Select(p => p.Ele).ToList();

        new EmaSmoothingFilter(new EmaSmoothingFilter.Config(0.5)).Apply(points);

        Assert.Equal(lats, points.Select(p => p.Lat).ToList());
        Assert.Equal(lons, points.Select(p => p.Lon).ToList());
        Assert.NotEqual(elevationsBefore, points.Select(p => p.Ele).ToList());
    }

    [Fact]
    public void Coordinates_LeavesElevationUnchanged_AndReducesDeviationFromLine()
    {
        // Constant Lat of 50 with alternating GPS noise, Lon steps evenly along the track.
        // A ramped Lat would add EMA lag larger than the noise, so only the noisy axis is measured.
        var points = NoisyStraightTrack(200);
        var deviationBefore = TotalDeviation(points);
        var elevationsBefore = points.Select(p => p.Ele).ToList();

        new EmaSmoothingFilter(new EmaSmoothingFilter.Config(0.5, EmaTargets.Coordinates)).Apply(points);

        Assert.Equal(elevationsBefore, points.Select(p => p.Ele).ToList());
        Assert.True(TotalDeviation(points) < deviationBefore);
    }

    [Fact]
    public void Both_SmoothesElevationAndCoordinates()
    {
        var points = NoisyStraightTrack(200);
        var deviationBefore = TotalDeviation(points);
        var elevationsBefore = points.Select(p => p.Ele).ToList();

        new EmaSmoothingFilter(new EmaSmoothingFilter.Config(0.5, EmaTargets.Both)).Apply(points);

        Assert.NotEqual(elevationsBefore, points.Select(p => p.Ele).ToList());
        Assert.True(TotalDeviation(points) < deviationBefore);
    }

    [Theory]
    [InlineData(EmaTargets.Elevation)]
    [InlineData(EmaTargets.Coordinates)]
    [InlineData(EmaTargets.Both)]
    public void AnyTargets_LeavesTimeAndPinnedUnchanged(EmaTargets targets)
    {
        var points = Track(3);
        points[1].Ele = 120;
        points[1].Time = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc);
        points[2].Pinned = true;
        var times = points.Select(p => p.Time).ToList();
        var pinned = points.Select(p => p.Pinned).ToList();

        new EmaSmoothingFilter(new EmaSmoothingFilter.Config(0.5, targets)).Apply(points);

        Assert.Equal(times, points.Select(p => p.Time).ToList());
        Assert.Equal(pinned, points.Select(p => p.Pinned).ToList());
    }

    [Fact]
    public void AlphaOne_ChangesNothing()
    {
        var points = NoisyStraightTrack(20);
        var before = points.Select(p => (p.Lat, p.Lon, p.Ele)).ToList();

        new EmaSmoothingFilter(new EmaSmoothingFilter.Config(1, EmaTargets.Both)).Apply(points);

        Assert.Equal(before, points.Select(p => (p.Lat, p.Lon, p.Ele)).ToList());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.1)]
    [InlineData(1.1)]
    public void Constructor_AlphaOutsideZeroToOne_Throws(double alpha)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new EmaSmoothingFilter(new EmaSmoothingFilter.Config(alpha)));
    }

    [Fact]
    public void Json_WithoutTargets_DeserialisesAsElevation()
    {
        var config = JsonSerializer.Deserialize<IPointFilterConfig>(
            """{"FilterType":"EmaSmoothingFilter","value":{"Alpha":0.5}}""",
            Json
        );

        Assert.IsType<EmaSmoothingFilter.Config>(config);
        Assert.Equal(EmaTargets.Elevation, ((EmaSmoothingFilter.Config)config!).Value.Targets);
    }

    [Fact]
    public void Json_WithCoordinatesTargets_DeserialisesAsCoordinates()
    {
        var config = JsonSerializer.Deserialize<IPointFilterConfig>(
            """{"FilterType":"EmaSmoothingFilter","value":{"Alpha":0.5,"Targets":"Coordinates"}}""",
            Json
        );

        Assert.Equal(EmaTargets.Coordinates, ((EmaSmoothingFilter.Config)config!).Value.Targets);
    }

    [Fact]
    public void Factory_BuildsEmaFilterFromConfig()
    {
        // The factory reads the typed Value of the wrapper, so the config is set the same way PipelineDefaults does.
        var config = new EmaSmoothingFilter.Config(0.5, EmaTargets.Coordinates);
        var wrapped = config with { Value = config };

        Assert.IsType<EmaSmoothingFilter>(RouteVisualizationService.GpxFilterFactory.CreateFilter(wrapped));
    }

    [Fact]
    public void CleanupDefaults_HaveOneEmaConfig_TargetingElevation()
    {
        var emaConfigs = PipelineDefaults.CleanupFilters.OfType<EmaSmoothingFilter.Config>().ToList();

        var emaConfig = Assert.Single(emaConfigs);
        Assert.Equal(EmaTargets.Elevation, emaConfig.Targets);
    }

    static double TotalDeviation(IEnumerable<MutableGpxPoint> points) =>
        points.Sum(p => Math.Abs(p.Lat - StraightLat));

    // Lat alternates +-0.00005 around the straight line, Lon steps evenly along it.
    static List<MutableGpxPoint> NoisyStraightTrack(int count) =>
        [.. Enumerable.Range(0, count).Select(i => new MutableGpxPoint(
            StraightLat + (i % 2 == 0 ? 0.00005 : -0.00005),
            20 + i * 0.0001,
            100 + i
        ))];

    static List<MutableGpxPoint> Track(int count) =>
        [.. Enumerable.Range(0, count).Select(i => new MutableGpxPoint(StraightLat, 20 + i * 0.0001, 100 + i))];
}
