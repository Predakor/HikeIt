using System.Text.Json;
using Application.Commons.Abstractions;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests.Pipeline;

// Phase 0 tests for BE-2. They fail until PinLocalMaxima, Simplify and the CreateVisualization fix are implemented.
public class RouteFilterTests
{
    static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    [Fact]
    public void Simplify_StraightTrack_CollapsesToEndpoints()
    {
        var points = Track(100, 110, 120, 130, 140, 150);

        var result = Simplify(points, toleranceM: 1);

        Assert.Equal(2, result.Count);
        Assert.Same(points[0], result[0]);
        Assert.Same(points[^1], result[^1]);
    }

    [Fact]
    public void Simplify_NeverRemovesPinnedPoint()
    {
        var points = Track(100, 110, 120, 130, 140, 150);
        points[2].Pinned = true;

        var result = Simplify(points, toleranceM: 1000);

        Assert.Contains(points[2], result);
    }

    [Fact]
    public void Simplify_KeepsPointsThatDeviateMoreThanTolerance()
    {
        var points = Track(100, 100, 300, 100, 100);

        var result = Simplify(points, toleranceM: 5);

        Assert.Contains(points[2], result);
    }

    [Fact]
    public void Simplify_ZeroOrOnePoint_ReturnsInputUnchanged()
    {
        Assert.Empty(Simplify(Track(), toleranceM: 5));
        Assert.Single(Simplify(Track(100), toleranceM: 5));
    }

    [Fact]
    public void PinLocalMaxima_PinsSummitAndEndpoints()
    {
        var points = Track(100, 110, 130, 120, 110, 100);

        new PinLocalMaximaFilter(new PinLocalMaximaFilter.Config(true)).Apply(points);

        Assert.True(points[0].Pinned);
        Assert.True(points[2].Pinned);
        Assert.True(points[^1].Pinned);
        Assert.False(points[1].Pinned);
        Assert.False(points[3].Pinned);
    }

    [Fact]
    public void PinThenSimplify_PlateauSummitSurvives()
    {
        var points = Track(100, 120, 130, 130, 130, 120, 100);

        new PinLocalMaximaFilter(new PinLocalMaximaFilter.Config(true)).Apply(points);
        var result = Simplify(points, toleranceM: 1000);

        Assert.Contains(result, p => p.Ele == 130);
    }

    [Fact]
    public void Pinned_SurvivesLaterElevationFilters()
    {
        var points = Track(100, 110, 130, 120, 110, 100);
        points[2].Pinned = true;

        CreateFilter("""{"FilterType":"MaxSpikeFilter","value":{"MaxSpike":5}}""").Apply(points);

        Assert.True(points[2].Pinned);
    }

    [Fact]
    public void CreateVisualization_UsesListReturnedByFilter()
    {
        var points = Track(100, 110, 120, 130, 140, 150).Select(p => new GpxPoint(p.Lat, p.Lon, p.Ele)).ToList();
        var service = new RouteVisualizationService(null!);

        var result = service.GetRouteVisualization(
            points,
            [CreateConfig("""{"FilterType":"SimplifyFilter","value":{"ToleranceM":1}}""")]
        );

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value!.Points.Count);
    }

    [Theory]
    [InlineData("""{"FilterType":"SimplifyFilter","value":{"ToleranceM":3}}""", typeof(SimplifyFilter.Config))]
    [InlineData("""{"FilterType":"PinLocalMaximaFilter","value":{"Enabled":true}}""", typeof(PinLocalMaximaFilter.Config))]
    public void FilterConfig_DeserialisesByDiscriminator(string json, Type expected)
    {
        var config = CreateConfig(json);

        Assert.IsType(expected, config);
    }

    [Fact]
    public void FilterConfig_SimplifyValue_RoundTrips()
    {
        var config = (SimplifyFilter.Config)CreateConfig("""{"FilterType":"SimplifyFilter","value":{"ToleranceM":3}}""");

        Assert.Equal(3, config.Value.ToleranceM);
    }

    static IFilterConfig CreateConfig(string json) => JsonSerializer.Deserialize<IFilterConfig>(json, Json)!;

    static IFilter<MutableGpxPoint> CreateFilter(string json) =>
        RouteVisualizationService.GpxFilterFactory.CreateFilter(CreateConfig(json));

    static IList<MutableGpxPoint> Simplify(IList<MutableGpxPoint> points, double toleranceM) =>
        new SimplifyFilter(new SimplifyFilter.Config(toleranceM)).Apply(points);

    // Points ~11m apart along a straight line, so equal elevation steps give a straight 3D line.
    static List<MutableGpxPoint> Track(params double[] elevations) =>
        [.. elevations.Select((e, i) => new MutableGpxPoint(50 + i * 0.0001, 20, e))];
}
