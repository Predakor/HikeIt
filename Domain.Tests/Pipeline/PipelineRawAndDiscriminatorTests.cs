using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Geography.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Domain.Tests.Pipeline;

// Tests for the raw run (untouched file) and the cache discriminator. Fake stages, orchestration only.
public class PipelineRawAndDiscriminatorTests
{
    private static readonly Guid TripId = Guid.NewGuid();

    [Fact]
    public async Task RawNotRequested_ResponseHasNoRaw()
    {
        var (service, _) = Create();

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        Assert.Null(response!.Raw);
    }

    [Fact]
    public async Task RawRequested_ReturnsEveryRequestedStage()
    {
        var (service, _) = Create();

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig(), null, IncludeRaw: true), default);

        Assert.NotNull(response!.Raw);
        Assert.All(StageIds.All, stage => Assert.True(response.Raw![StageIds.Key(stage)].Ok));
    }

    [Fact]
    public async Task RawRequested_RunsWithCleanupFiltersEmpty_AndOtherSectionsDefault()
    {
        var (service, stages) = Create();

        await service.PreviewAsync(TripId, new(new PipelineConfig(), null, IncludeRaw: true), default);

        var rawConfigs = stages[StageId.Route].ReceivedConfigs.Where(c => c.Cleanup?.Filters is { Count: 0 }).ToList();
        Assert.Single(rawConfigs);
        Assert.Null(rawConfigs[0].Route);
        Assert.Null(rawConfigs[0].ElevationProfile);
        Assert.Null(rawConfigs[0].PeakMatching);
    }

    [Fact]
    public async Task RawRequested_StagesRespectTheStagesList()
    {
        var (service, _) = Create();

        var response = await service.PreviewAsync(
            TripId,
            new(new PipelineConfig(), [StageId.Route], IncludeRaw: true),
            default
        );

        Assert.Equal([StageIds.Key(StageId.Route)], response!.Raw!.Keys);
    }

    [Fact]
    public async Task Raw_IsCachedAcrossRequests()
    {
        var (service, stages) = Create();
        await service.PreviewAsync(TripId, new(new PipelineConfig(), null, IncludeRaw: true), default);

        var second = await service.PreviewAsync(TripId, new(new PipelineConfig(), null, IncludeRaw: true), default);

        Assert.All(StageIds.All, stage => Assert.True(second!.Raw![StageIds.Key(stage)].CacheHit));
        Assert.Equal(2, stages[StageId.Cleanup].Runs); // once for the default config, once for raw, nothing on the repeat
    }

    [Fact]
    public async Task ChangedDiscriminator_RecomputesTheStageAndItsDependents()
    {
        var (service, stages) = Create();
        await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        stages[StageId.Cleanup].Discriminator = "settings-v2";
        var response = await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        Assert.All(StageIds.All, stage => Assert.False(response!.Results[StageIds.Key(stage)].CacheHit));
        Assert.All(StageIds.All, stage => Assert.Equal(2, stages[stage].Runs));
    }

    [Fact]
    public async Task UnchangedDiscriminator_KeepsCachedResults()
    {
        var (service, stages) = Create();
        stages[StageId.Cleanup].Discriminator = "settings-v1";
        await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        var response = await service.PreviewAsync(TripId, new(new PipelineConfig()), default);

        Assert.True(response!.Results[StageIds.Key(StageId.Cleanup)].CacheHit);
        Assert.Equal(1, stages[StageId.Cleanup].Runs);
    }

    private static (PipelinePreviewService Service, Dictionary<StageId, FakeStage> Stages) Create()
    {
        var stages = StageIds.All.ToDictionary(s => s, s => new FakeStage(s));
        var service = new PipelinePreviewService(
            new FakeSource(),
            stages.Values,
            new MemoryCache(new MemoryCacheOptions())
        );

        return (service, stages);
    }

    private sealed class FakeSource : IPipelineSource
    {
        public Task<PipelineInput?> LoadAsync(Guid tripId, CancellationToken cancellationToken) =>
            Task.FromResult<PipelineInput?>(new PipelineInput("user-a/file", [new GpxPoint(50, 20, 100)]));
    }

    private sealed class FakeStage(StageId id) : IPipelineStage
    {
        private static readonly Dictionary<StageId, Func<PipelineConfig, object?>> ConfigSelectors = new()
        {
            [StageId.Cleanup] = config => config.Cleanup,
            [StageId.Analytics] = _ => null,
            [StageId.ElevationProfile] = config => config.ElevationProfile,
            [StageId.PeakMatching] = config => config.PeakMatching,
            [StageId.Route] = config => config.Route,
        };

        public StageId Id { get; } = id;
        public int Runs { get; private set; }
        public string Discriminator { get; set; } = string.Empty;
        public List<PipelineConfig> ReceivedConfigs { get; } = [];

        public object? SelectConfig(PipelineConfig config) => ConfigSelectors[Id](config);

        public Task<string> CacheDiscriminatorAsync(CancellationToken cancellationToken) => Task.FromResult(Discriminator);
    }
}