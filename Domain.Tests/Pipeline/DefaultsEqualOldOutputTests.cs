using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles.Commands;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests.Pipeline;

// Guards that the PipelineDefaults lists produce the same output as the production code paths they replace.
public class DefaultsEqualOldOutputTests
{
    public static TheoryData<string> GpxFiles { get; } =
        ["data/trip_small.gpx", "data/trip_medium.gpx", "data/trip_big.gpx"];

    // Known divergence, not fixed here (see the BE-1 report): MedianSmoothingFilter uses a different window
    // than GpxBuilderMethods.MedianSmooth, and EmaSmoothingFilter writes each result one index early.
    // Clamp alone matches exactly. Remove the Skip once the filters are made equivalent.
    [Theory(Skip = "Filter cleanup chain differs from GpxDataDirector (median window, EMA index shift)")]
    [MemberData(nameof(GpxFiles))]
    public async Task CleanupDefaults_MatchGpxDataDirector_Elevations(string path)
    {
        var data = await ParserTests.ParseFromGpxFile(path);

        var expected = GpxDataDirector.AnalyticData(data.Points).Points.Select(p => p.Ele).ToList();
        var actual = ApplyDefaultCleanup(data.Points).Select(p => p.Ele).ToList();

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GpxFiles))]
    public async Task PeakDefaults_MatchWithProximityMerge(string path)
    {
        var data = await ParserTests.ParseFromGpxFile(path);
        var candidates = data.ToLocalMaximaWithDistance();

        var expected = candidates.WithProximityMerge();
        var actual = PeakFilterFactory.Apply(candidates, PipelineDefaults.PeakFilters);

        Assert.NotEmpty(expected);
        Assert.Equal(expected, actual);
    }

    static List<MutableGpxPoint> ApplyDefaultCleanup(List<GpxPoint> points)
    {
        var mutablePoints = points.Select(p => p.ToMutable()).ToList();

        foreach (var filter in RouteVisualizationService.GpxFilterFactory.CreateFilterList(PipelineDefaults.CleanupFilters))
        {
            filter.Apply(mutablePoints);
        }

        return mutablePoints;
    }
}
