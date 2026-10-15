using System.Text.Json;
using Application.Commons.Abstractions;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Trips.Analytics.Shared.Filters;

namespace Domain.Tests.Pipeline;

public class PipelineContractTests
{
    static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    [Fact]
    public void Fixture_DeserialisesIntoResponse()
    {
        var json = File.ReadAllText("data/fixture.response.json");

        var response = JsonSerializer.Deserialize<PipelinePreviewResponse>(json, Json);

        Assert.NotNull(response);
        foreach (var stage in StageIds.All)
        {
            Assert.True(response.Results.ContainsKey(StageIds.Key(stage)), $"results missing {stage}");
            Assert.True(response.Baseline.ContainsKey(StageIds.Key(stage)), $"baseline missing {stage}");
        }
    }

    [Fact]
    public void Fixture_StageResultKeysMatchStageIds()
    {
        var json = File.ReadAllText("data/fixture.response.json");
        var response = JsonSerializer.Deserialize<PipelinePreviewResponse>(json, Json)!;

        foreach (var (key, result) in response.Results)
        {
            Assert.Equal(key, StageIds.Key(result.Stage));
        }
    }

    [Fact]
    public void StageIds_Keys_AreCamelCase()
    {
        Assert.Equal("elevationProfile", StageIds.Key(StageId.ElevationProfile));
        Assert.Equal("peakMatching", StageIds.Key(StageId.PeakMatching));
    }

    [Fact]
    public void StageIds_EveryStageExceptCleanup_DependsOnlyOnCleanup()
    {
        Assert.Empty(StageIds.DependenciesOf(StageId.Cleanup));

        foreach (var stage in StageIds.All.Where(s => s != StageId.Cleanup))
        {
            Assert.Equal([StageId.Cleanup], StageIds.DependenciesOf(stage));
        }
    }

    [Fact]
    public void Request_PeakFilterInPointList_IsRejected()
    {
        const string json = """
            { "config": { "route": { "filters": [
              { "FilterType": "MergeNearbyByDistanceFilter", "value": { "DistanceM": 50 } }
            ] } } }
            """;

        Assert.ThrowsAny<JsonException>(() => JsonSerializer.Deserialize<PipelinePreviewRequest>(json, Json));
    }

    [Fact]
    public void Request_PointFilterInPeakList_IsRejected()
    {
        const string json = """
            { "config": { "peakMatching": { "filters": [
              { "FilterType": "SimplifyFilter", "value": { "ToleranceM": 3 } }
            ] } } }
            """;

        Assert.ThrowsAny<JsonException>(() => JsonSerializer.Deserialize<PipelinePreviewRequest>(json, Json));
    }

    [Fact]
    public void Request_PeakFilters_DeserialiseInPeakList()
    {
        const string json = """
            { "config": { "peakMatching": { "proximityRadiusM": 80, "filters": [
              { "FilterType": "MergeNearbyByDistanceFilter", "value": { "DistanceM": 50 } },
              { "FilterType": "MergeNearbyByElevationFilter", "value": { "ElevationDeltaM": 30 } }
            ] } } }
            """;

        var request = JsonSerializer.Deserialize<PipelinePreviewRequest>(json, Json)!;

        Assert.Equal(80f, request.Config.PeakMatching!.ProximityRadiusM);
        Assert.IsType<MergeNearbyByDistanceFilter.Config>(request.Config.PeakMatching.Filters![0]);
        Assert.IsType<MergeNearbyByElevationFilter.Config>(request.Config.PeakMatching.Filters[1]);
    }

    [Fact]
    public void Request_WithNewFilters_Deserialises()
    {
        const string json = """
            {
              "config": {
                "cleanup": { "filters": [{ "FilterType": "MaxSpikeFilter", "value": { "MaxSpike": 5 } }] },
                "route": { "filters": [
                  { "FilterType": "PinLocalMaximaFilter", "value": { "Enabled": true } },
                  { "FilterType": "SimplifyFilter", "value": { "ToleranceM": 2.5 } }
                ] }
              },
              "stages": ["cleanup", "route"]
            }
            """;

        var request = JsonSerializer.Deserialize<PipelinePreviewRequest>(json, Json)!;

        Assert.IsType<MaxSpikeFilter.Config>(request.Config.Cleanup!.Filters![0]);
        Assert.Null(request.Config.PeakMatching);
        Assert.Equal([StageId.Cleanup, StageId.Route], request.Stages);
        Assert.IsType<PinLocalMaximaFilter.Config>(request.Config.Route!.Filters![0]);
        Assert.IsType<SimplifyFilter.Config>(request.Config.Route.Filters[1]);
    }
}
