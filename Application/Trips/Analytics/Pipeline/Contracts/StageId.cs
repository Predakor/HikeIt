using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Trips.Analytics.Pipeline.Contracts;

[JsonConverter(typeof(StageIdJsonConverter))]
public enum StageId
{
    Cleanup,
    Analytics,
    ElevationProfile,
    PeakMatching,
    Route,
}

public sealed class StageIdJsonConverter : JsonStringEnumConverter<StageId>
{
    public StageIdJsonConverter()
        : base(JsonNamingPolicy.CamelCase) { }
}

public static class StageIds
{
    // Every stage depends on cleanup; there are no other edges.
    private static readonly IReadOnlyDictionary<StageId, StageId[]> Dependencies = new Dictionary<StageId, StageId[]>
    {
        [StageId.Cleanup] = [],
        [StageId.Analytics] = [StageId.Cleanup],
        [StageId.ElevationProfile] = [StageId.Cleanup],
        [StageId.PeakMatching] = [StageId.Cleanup],
        [StageId.Route] = [StageId.Cleanup],
    };

    public static IReadOnlyList<StageId> All { get; } = [.. Enum.GetValues<StageId>()];

    public static IReadOnlyList<StageId> DependenciesOf(StageId stage) => Dependencies[stage];

    // Key used in the response dictionaries ("elevationProfile").
    public static string Key(StageId stage) => JsonNamingPolicy.CamelCase.ConvertName(stage.ToString());
}