using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Shared.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace Domain.Tests.Pipeline;

// Phase 2 tests for BE-3b (orchestrator, cache). They use fake stages, so they test orchestration only.
public class PipelinePreviewServiceTests
{
    private static readonly Guid TripId = Guid.NewGuid();

    [Fact]
    public async Task AllStages_ReturnResultsAndBaselineForEveryStage()
    {
        var (service, _) = Create();

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        Assert.NotNull(response);
        foreach (var stage in StageIds.All)
        {
            Assert.True(response.Results[StageIds.Key(stage)].Ok);
            Assert.True(response.Baseline[StageIds.Key(stage)].Ok);
        }
    }

    [Fact]
    public async Task RequestedSubset_ReturnsOnlyThoseStages_ButStillRunsDependencies()
    {
        var (service, stages) = Create();

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig(), [StageId.Route]), default);

        Assert.Equal([StageIds.Key(StageId.Route)], response!.Results.Keys);
        Assert.Equal([StageIds.Key(StageId.Route)], response.Baseline.Keys);
        Assert.Equal(1, stages[StageId.Cleanup].Runs);
        Assert.Equal(0, stages[StageId.Analytics].Runs);
    }

    [Fact]
    public async Task SameConfig_ComputesPreviewAndBaselineOnce()
    {
        var (service, stages) = Create();

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        Assert.All(StageIds.All, stage => Assert.Equal(1, stages[stage].Runs));
        Assert.True(response!.Baseline[StageIds.Key(StageId.Cleanup)].CacheHit
            || response.Results[StageIds.Key(StageId.Cleanup)].CacheHit);
    }

    [Fact]
    public async Task SecondRun_OnlyRouteConfigChanged_CleanupAndAnalyticsAreCacheHits()
    {
        var (service, stages) = Create();
        await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        var changed = new PipelineConfig(Route: new PipeConfig.Route([SimplifyConfig(3)]));
        var response = await service.PreviewAsync(TripId, new(changed), default);

        Assert.True(response!.Results[StageIds.Key(StageId.Cleanup)].CacheHit);
        Assert.True(response.Results[StageIds.Key(StageId.Analytics)].CacheHit);
        Assert.False(response.Results[StageIds.Key(StageId.Route)].CacheHit);
        Assert.Equal(1, stages[StageId.Cleanup].Runs);
        Assert.Equal(1, stages[StageId.Analytics].Runs);
        Assert.Equal(2, stages[StageId.Route].Runs);
    }

    [Fact]
    public async Task CleanupConfigChanged_RecomputesEveryDependentStage()
    {
        var (service, stages) = Create();
        await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        var changed = new PipelineConfig(Cleanup: new PipeConfig.Cleanup([SimplifyConfig(1)]));
        var response = await service.PreviewAsync(TripId, new(changed), default);

        Assert.All(StageIds.All, stage => Assert.False(response!.Results[StageIds.Key(stage)].CacheHit));
        Assert.All(StageIds.All, stage => Assert.Equal(2, stages[stage].Runs));
    }

    [Fact]
    public async Task OtherCacheScope_DoesNotShareCache()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        var stages = FakeStages();
        var first = new PipelinePreviewService(new FakeSource("user-a/file"), stages.Values, cache);
        var second = new PipelinePreviewService(new FakeSource("user-b/file"), stages.Values, cache);

        await first.PreviewAsync(TripId, new(new PipelineConfig()), default);
        var response = await second.PreviewAsync(TripId, new(new PipelineConfig()), default);

        Assert.False(response!.Results[StageIds.Key(StageId.Cleanup)].CacheHit);
        Assert.Equal(2, stages[StageId.Cleanup].Runs);
    }

    [Fact]
    public async Task FailingStage_IsReportedAndDoesNotAffectOthers()
    {
        var (service, stages) = Create();
        stages[StageId.Analytics].Failure = new InvalidOperationException("boom");

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        var analytics = response!.Results[StageIds.Key(StageId.Analytics)];
        Assert.False(analytics.Ok);
        Assert.Contains("boom", analytics.Error);
        Assert.True(response.Results[StageIds.Key(StageId.Route)].Ok);
    }

    [Fact]
    public async Task FailingCleanup_FailsDependentsWithoutRunningThem()
    {
        var (service, stages) = Create();
        stages[StageId.Cleanup].Failure = new InvalidOperationException("bad file");

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        Assert.False(response!.Results[StageIds.Key(StageId.Cleanup)].Ok);
        Assert.All(
            StageIds.All.Where(s => s != StageId.Cleanup),
            stage =>
            {
                Assert.False(response.Results[StageIds.Key(stage)].Ok);
                Assert.Equal(0, stages[stage].Runs);
            }
        );
    }

    [Fact]
    public async Task DownstreamStage_ReceivesUpstreamOutput_EvenFromCache()
    {
        var (service, stages) = Create();
        await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        var changed = new PipelineConfig(Route: new PipeConfig.Route([SimplifyConfig(3)]));
        await service.PreviewAsync(TripId, new(changed), default);

        Assert.Equal("cleanup-output", stages[StageId.Route].LastUpstreamInternal);
    }

    [Fact]
    public async Task MissingTripFile_ReturnsNull()
    {
        var service = new PipelinePreviewService(
            new FakeSource(null),
            FakeStages().Values,
            new MemoryCache(new MemoryCacheOptions())
        );

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        Assert.Null(response);
    }

    [Fact]
    public async Task ResultMetadata_IsFilled()
    {
        var (service, _) = Create();

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        var route = response!.Results[StageIds.Key(StageId.Route)];
        Assert.Equal(StageId.Route, route.Stage);
        Assert.Equal(3, route.PointCount);
        Assert.True(route.PayloadBytes > 0);
        Assert.True(route.DurationMs >= 0);
    }

    private static (PipelinePreviewService Service, Dictionary<StageId, FakeStage> Stages) Create()
    {
        var stages = FakeStages();
        var service = new PipelinePreviewService(
            new FakeSource("user-a/file"),
            stages.Values,
            new MemoryCache(new MemoryCacheOptions())
        );

        return (service, stages);
    }

    private static Dictionary<StageId, FakeStage> FakeStages() => StageIds.All.ToDictionary(s => s, s => new FakeStage(s));

    private static IPointFilterConfig SimplifyConfig(double toleranceM) =>
        new SimplifyFilter.Config(toleranceM) with { Value = new SimplifyFilter.Config(toleranceM) };

    private sealed class FakeSource(string? scope) : IPipelineSource
    {
        public Task<PipelineInput?> LoadAsync(Guid tripId, CancellationToken cancellationToken) =>
            Task.FromResult(scope is null ? null : new PipelineInput(scope, [new GpxPoint(50, 20, 100)]));
    }

    // Counts runs; selects the matching section of the config like a real stage would.
    private sealed class FakeStage(StageId id) : IPipelineStage
    {
        public StageId Id { get; } = id;
        public int Runs { get; private set; }
        public Exception? Failure { get; set; }
        public object? LastUpstreamInternal { get; private set; }

        private static readonly Dictionary<StageId, Func<PipelineConfig, object?>> ConfigSelectors = new()
        {
            [StageId.Cleanup] = config => config.Cleanup,
            [StageId.Analytics] = _ => null,
            [StageId.ElevationProfile] = config => config.ElevationProfile,
            [StageId.PeakMatching] = config => config.PeakMatching,
            [StageId.Route] = config => config.Route,
        };

        public object? SelectConfig(PipelineConfig config) => ConfigSelectors[Id](config);
    }
}