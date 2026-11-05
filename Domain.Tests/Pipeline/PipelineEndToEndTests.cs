using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Stages;
using Application.Trips.Analytics.RouteVisualizations;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Shared.Filters;
using Domain.Trips.Root.Builders.GpxDataBuilder;
using Microsoft.Extensions.Caching.Memory;

namespace Domain.Tests.Pipeline;

// Real orchestrator with the real cleanup, analytics, profile and route stages on a stored-file fixture.
// Peak matching needs the database, so it is left out of the requested stages.
public class PipelineEndToEndTests
{
    static readonly StageId[] DatabaseFreeStages =
        [StageId.Cleanup, StageId.Analytics, StageId.ElevationProfile, StageId.Route];

    [Fact]
    public async Task DefaultConfig_CleanupMatchesProduction_AndPreviewEqualsBaseline()
    {
        var (service, points) = await CreateAsync();

        var response = await service.PreviewAsync(Guid.NewGuid(), new(new PipelineConfig(), DatabaseFreeStages), default);

        var cleanup = response!.Results[StageIds.Key(StageId.Cleanup)];
        var productionCount = GpxDataDirector.AnalyticData([.. points]).Points.Count;
        Assert.True(cleanup.Ok);
        Assert.Equal(productionCount, ((StageData.Cleanup)cleanup.Data!).PointCountOut);
        Assert.All(DatabaseFreeStages, stage =>
        {
            var key = StageIds.Key(stage);
            Assert.True(response.Results[key].Ok, $"{stage}: {response.Results[key].Error}");
            Assert.Equal(response.Baseline[key].PayloadBytes, response.Results[key].PayloadBytes);
        });

        Assert.True(response.Results[StageIds.Key(StageId.Route)].PointCount < productionCount);
    }

    [Fact]
    public async Task SimplifyOnProfileAndRoute_ShrinksThem_AndKeepsAnalyticsEqualToBaseline()
    {
        var (service, _) = await CreateAsync();
        var config = new PipelineConfig(
            ElevationProfile: new PipeConfig.Profile([PinAndSimplify()[0], PinAndSimplify()[1]]),
            Route: new PipeConfig.Route(PinAndSimplify())
        );

        var response = await service.PreviewAsync(Guid.NewGuid(), new(config, DatabaseFreeStages), default);

        var profile = (ElevationProfileDto)response!.Results[StageIds.Key(StageId.ElevationProfile)].Data!;
        var baselineProfile = (ElevationProfileDto)response.Baseline[StageIds.Key(StageId.ElevationProfile)].Data!;
        Assert.True(profile.Gains.Length < baselineProfile.Gains.Length);

        var route = (RoutePath)response.Results[StageIds.Key(StageId.Route)].Data!;
        Assert.True(route.Points.Count < response.Results[StageIds.Key(StageId.Cleanup)].PointCount);

        var analytics = response.Results[StageIds.Key(StageId.Analytics)];
        Assert.True(analytics.CacheHit || response.Baseline[StageIds.Key(StageId.Analytics)].CacheHit);
        Assert.Equal(response.Baseline[StageIds.Key(StageId.Analytics)].PayloadBytes, analytics.PayloadBytes);
    }

    [Fact]
    public async Task Summit_SurvivesPinnedSimplification_InTheProfile()
    {
        var (service, _) = await CreateAsync();
        var config = new PipelineConfig(ElevationProfile: new PipeConfig.Profile(PinAndSimplify(toleranceM: 50)));

        var response = await service.PreviewAsync(
            Guid.NewGuid(),
            new(config, [StageId.Cleanup, StageId.ElevationProfile]),
            default
        );

        var profile = (ElevationProfileDto)response!.Results[StageIds.Key(StageId.ElevationProfile)].Data!;
        var highestInProfile = profile.Gains.Aggregate(profile.Start.Ele, (ele, gain) => Math.Max(ele, ele + gain.Ele)) ;
        var cleaned = (StageData.Cleanup)response.Results[StageIds.Key(StageId.Cleanup)].Data!;
        Assert.True(cleaned.PointCountOut > profile.Gains.Length);
        Assert.True(highestInProfile > profile.Start.Ele);
    }

    static IPointFilterConfig[] PinAndSimplify(double toleranceM = 5) =>
    [
        new PinLocalMaximaFilter.Config(true) with { Value = new PinLocalMaximaFilter.Config(true) },
        new SimplifyFilter.Config(toleranceM) with { Value = new SimplifyFilter.Config(toleranceM) },
    ];

    static async Task<(PipelinePreviewService Service, IReadOnlyList<GpxPoint> Points)> CreateAsync()
    {
        var data = await ParserTests.ParseFromGpxFile("data/trip_small.gpx");
        IPipelineStage[] stages = [new CleanupStage(), new AnalyticsStage(), new ElevationProfileStage()];
        var routeStage = new RouteStage(new RouteVisualizationService(null!));
        var service = new PipelinePreviewService(
            new FileSource(data.Points),
            [.. stages, routeStage],
            new MemoryCache(new MemoryCacheOptions())
        );

        return (service, data.Points);
    }

    sealed class FileSource(IReadOnlyList<GpxPoint> points) : IPipelineSource
    {
        public Task<PipelineInput?> LoadAsync(Guid tripId, CancellationToken cancellationToken) =>
            Task.FromResult<PipelineInput?>(new PipelineInput("test/file", points));
    }
}
