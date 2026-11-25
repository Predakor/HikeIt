using System.Text.Json.Serialization;
using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Contracts;

public static class PipeConfig
{
    public sealed record Cleanup(IReadOnlyList<IPointFilterConfig>? Filters = null);

    public sealed record Profile(IReadOnlyList<IPointFilterConfig>? Filters = null);

    public sealed record Peaks(IReadOnlyList<IPeakFilterConfig>? Filters = null);

    public sealed record Route(IReadOnlyList<IPointFilterConfig>? Filters = null);

    public sealed record Terrain(IReadOnlyList<ITerrainFilterConfig>? Filters = null);

    public sealed record Pace(IReadOnlyList<IPaceFilterConfig>? Filters = null);
}

public sealed record PipelineConfig(
    PipeConfig.Cleanup? Cleanup = null,
    PipeConfig.Profile? ElevationProfile = null,
    PipeConfig.Peaks? PeakMatching = null,
    PipeConfig.Route? Route = null,
    PipeConfig.Terrain? Terrain = null,
    PipeConfig.Pace? Pace = null
);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ComparisonTarget
{
    CurrentProd,
    Raw,
}

public sealed record PipelinePreviewRequest(
    PipelineConfig Config,
    IReadOnlyList<StageId>? Stages = null,
    ComparisonTarget Target = ComparisonTarget.CurrentProd,
    bool IncludeSteps = false
);
