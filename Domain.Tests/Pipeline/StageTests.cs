using Application.Locations.Peaks;
using Application.Locations.Regions;
using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Stages;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.Common.Abstractions;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;
using Domain.ReachedPeaks.Builders;
using Domain.Trips.Analytics.ElevationProfiles.Commands;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Route.Builders;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Analytics.Time;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Tests.Pipeline;

public class StageTests
{
    [Fact]
    public async Task Cleanup_Default_ElevationsMatchGpxDataDirector()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");

        var output = await RunCleanup(data.Points);

        var expected = GpxDataDirector.AnalyticData(data.Points).Points.Select(p => p.Ele).ToList();
        var actual = CleanedPoints(output).Select(p => p.Ele).ToList();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Cleanup_WithSimplify_ReturnsFewerPointsAndReportsCount()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");

        var defaultOutput = await RunCleanup(data.Points);
        var simplified = await RunCleanup(data.Points, new PipeConfig.Cleanup([Simplify(5)]));

        Assert.True(simplified.PointCount < defaultOutput.PointCount);
        var cleanup = Assert.IsType<StageData.Cleanup>(simplified.Data);
        Assert.Equal(data.Points.Count, cleanup.PointCountIn);
        Assert.Equal(simplified.PointCount, cleanup.PointCountOut);
        Assert.Equal(CleanedPoints(simplified).Count, cleanup.PointCountOut);
    }

    [Fact]
    public async Task Analytics_Default_RouteEqualsRouteAnalyticFactoryOnCleanedData()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var context = await CleanupContext(data.Points);

        var output = await new AnalyticsStage().RunAsync(context, new PipelineConfig(), default);

        var cleaned = CleanupStage.CleanedDataFrom(context);
        var expected = RouteAnalyticFactory.Create(cleaned);
        var analytics = Assert.IsType<StageData.Analytics>(output.Data);
        Assert.Equal(expected.TotalDistanceMeters, analytics.Route.TotalDistanceMeters);
        Assert.Equal(expected.TotalAscentMeters, analytics.Route.TotalAscentMeters);
        Assert.Equal(expected.TotalDescentMeters, analytics.Route.TotalDescentMeters);
        Assert.Equal(expected.HighestElevationMeters, analytics.Route.HighestElevationMeters);
        Assert.Equal(expected.LowestElevationMeters, analytics.Route.LowestElevationMeters);
        Assert.Equal(expected.AverageSlopePercent, analytics.Route.AverageSlopePercent);
        Assert.Equal(expected.AverageAscentSlopePercent, analytics.Route.AverageAscentSlopePercent);
        Assert.Equal(expected.AverageDescentSlopePercent, analytics.Route.AverageDescentSlopePercent);
        Assert.NotNull(analytics.Time);
        Assert.Equal(cleaned.Points.Count, output.PointCount);
    }

    [Fact]
    public async Task ElevationProfile_ExplicitEmptyFilters_KeepsEveryCleanedPoint()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var context = await CleanupContext(data.Points);
        var cleanedCount = CleanupStage.CleanedDataFrom(context).Points.Count;
        var config = new PipelineConfig(ElevationProfile: new PipeConfig.Profile([]));

        var output = await new ElevationProfileStage().RunAsync(context, config, default);

        var profile = Assert.IsType<ElevationProfileDto>(output.Data);
        // ToGains yields one gain per step between points, so N points give N - 1 gains.
        Assert.Equal(cleanedCount - 1, profile.Gains.Length);
        Assert.Equal(cleanedCount, output.PointCount);
    }

    [Fact]
    public async Task ElevationProfile_Default_AppliesTheElevationProfileSettingFallback()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var context = await CleanupContext(data.Points);
        var stage = new ElevationProfileStage();

        var defaultOutput = await stage.RunAsync(context, new PipelineConfig(), default);
        var explicitConfig = new PipelineConfig(ElevationProfile: new PipeConfig.Profile(PipelineDefaults.ElevationProfileFilters));
        var explicitOutput = await stage.RunAsync(context, explicitConfig, default);

        Assert.Equal(explicitOutput.PointCount, defaultOutput.PointCount);
    }

    [Fact]
    public async Task ElevationProfile_PinAndSimplify_HasFewerGainsThanUnfiltered()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var context = await CleanupContext(data.Points);
        var stage = new ElevationProfileStage();

        var unfilteredOutput = await stage.RunAsync(
            context,
            new PipelineConfig(ElevationProfile: new PipeConfig.Profile([])),
            default
        );
        var config = new PipelineConfig(ElevationProfile: new PipeConfig.Profile([PinLocalMaxima(), Simplify(5)]));
        var simplified = await stage.RunAsync(context, config, default);

        var unfilteredProfile = (ElevationProfileDto)unfilteredOutput.Data!;
        var simplifiedProfile = (ElevationProfileDto)simplified.Data!;
        Assert.True(simplifiedProfile.Gains.Length < unfilteredProfile.Gains.Length);
        Assert.Equal(simplified.PointCount, simplifiedProfile.Gains.Length + 1);
    }

    [Fact]
    public async Task PeakMatching_Default_QueriesWithProximityMergeCandidatesAndTouchesNothingElse()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var context = await CleanupContext(data.Points);
        var fake = new FakePeaksQueryService();
        var cleaned = CleanupStage.CleanedDataFrom(context);

        var output = await new PeakMatchingStage(fake).RunAsync(context, new PipelineConfig(), default);

        var expectedCandidates = cleaned.ToLocalMaximaWithDistance().WithProximityMerge();
        Assert.NotEmpty(expectedCandidates);
        Assert.Equal(expectedCandidates.Count, fake.CandidateCounts.Single());
        Assert.Equal(PipelineDefaults.PeakProximityRadiusM, fake.Radius);
        Assert.Equal(["GetPeaksWithinRadius"], fake.Calls);

        var peaks = Assert.IsType<StageData.PeakMatching>(output.Data);
        Assert.Equal(expectedCandidates.Count, peaks.CandidateCount);
        Assert.Empty(peaks.Matched);
        Assert.Equal(0, output.PointCount);
    }

    [Fact]
    public async Task PeakMatching_WhenPeakFound_MapsNameElevationAndCandidateDistance()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var context = await CleanupContext(data.Points);
        var candidate = CleanupStage.CleanedDataFrom(context).ToLocalMaximaWithDistance().WithProximityMerge()[0].BasePoint;

        // Roughly 40 m north of the candidate (1 degree of latitude is about 111.2 km).
        var peakPoint = new GpxPoint(candidate.Lat + 40.0 / 111_195.0, candidate.Lon, 1234);
        var fake = new FakePeaksQueryService { PeakToMatch = Peak.Create("Test Peak", peakPoint, 3) };

        var output = await new PeakMatchingStage(fake).RunAsync(context, new PipelineConfig(), default);

        var peaks = Assert.IsType<StageData.PeakMatching>(output.Data);
        var matched = Assert.Single(peaks.Matched);
        Assert.Equal("Test Peak", matched.Name);
        Assert.Equal(1234d, matched.Elevation);
        Assert.InRange(matched.DistanceM, 39d, 41d);
        Assert.True(matched.DistanceM < PipelineDefaults.PeakProximityRadiusM);
        Assert.Equal(["GetPeaksWithinRadius"], fake.Calls);
        Assert.Equal(1, output.PointCount);
    }

    [Fact]
    public async Task Route_WithSimplifyFilter_ReturnsFewerPointsThanCleaned()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        var context = await CleanupContext(data.Points);
        var cleanedCount = CleanupStage.CleanedDataFrom(context).Points.Count;
        var stage = new RouteStage(new FakeRouteVisualizationService());
        var config = new PipelineConfig(Route: new PipeConfig.Route([Simplify(5)]));

        var output = await stage.RunAsync(context, config, default);

        var path = Assert.IsType<RoutePath>(output.Data);
        Assert.True(path.Points.Count < cleanedCount);
        Assert.Equal(path.Points.Count, output.PointCount);
    }

    private static async Task<StageOutput> RunCleanup(List<GpxPoint> points, PipeConfig.Cleanup? cleanup = null)
    {
        var config = new PipelineConfig(Cleanup: cleanup);
        return await new CleanupStage().RunAsync(
            new StageContext(new PipelineInput("test", points), new Dictionary<StageId, StageOutput>()),
            config,
            default
        );
    }

    // Runs the cleanup stage with its default config and returns a context that downstream stages can read.
    private static async Task<StageContext> CleanupContext(List<GpxPoint> points)
    {
        var input = new PipelineInput("test", points);
        var cleanup = await new CleanupStage().RunAsync(
            new StageContext(input, new Dictionary<StageId, StageOutput>()),
            new PipelineConfig(),
            default
        );

        return new StageContext(input, new Dictionary<StageId, StageOutput> { [StageId.Cleanup] = cleanup });
    }

    private static List<GpxPoint> CleanedPoints(StageOutput output) => ((AnalyticData)output.Internal!).Points;

    // Filters built like PipelineDefaults: the config value must be set, otherwise GetValue() returns null.
    private static IPointFilterConfig Simplify(double toleranceM) =>
        new SimplifyFilter.Config(toleranceM) with { Value = new SimplifyFilter.Config(toleranceM) };

    private static IPointFilterConfig PinLocalMaxima() =>
        new PinLocalMaximaFilter.Config(true) with { Value = new PinLocalMaximaFilter.Config(true) };

    private sealed class FakePeaksQueryService : IPeaksQueryService
    {
        public List<string> Calls { get; } = [];
        public List<int> CandidateCounts { get; } = [];
        public float Radius { get; private set; }
        public Peak? PeakToMatch { get; init; }

        public Task<Result<PeakDto.Complete>> GetByIdAsync(int id) => throw new NotSupportedException();

        public Task<Result<IEnumerable<PeakDto.Complete>>> GetAllAsync() => throw new NotSupportedException();

        public Task<Result<Peak>> GetPeakWithinRadius(GpxPoint point, float radius) => throw new NotSupportedException();

        public Task<Result<Peak>> GetPeakWithNameFromRegion(string name, int regionId) => throw new NotSupportedException();

        // Used by the stage to recover the matched peak's location around its candidate.
        public Task<Result<List<Peak>>> GetPeaksWithinRadius(IEnumerable<GpxPoint> points, float radius)
        {
            Calls.Add("NearbyPeaks");
            List<Peak> nearby = PeakToMatch is null ? [] : [PeakToMatch];
            return Task.FromResult<Result<List<Peak>>>(nearby);
        }
    }

    // Delegates to the real service; the filter overload never reads app settings, so no settings service is needed.
    private sealed class FakeRouteVisualizationService : IRouteVisualizationService
    {
        private readonly RouteVisualizationService _inner = new(null!);

        public Task<Result<RoutePath>> GetRouteVisualizationAsync(IEnumerable<GpxPoint> points) =>
            throw new NotSupportedException("Settings-backed overload is not used by these tests.");
    }
}