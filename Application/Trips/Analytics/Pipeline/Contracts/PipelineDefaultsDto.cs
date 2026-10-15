using System.Text.Json.Serialization;
using Application.Commons.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Contracts;

// One filter in the wire shape of a request: { "FilterType": "SimplifyFilter", "value": { "ToleranceM": 3 } }.
// Plain data on purpose: the polymorphic filter configs do not serialise back cleanly (FilterType clashes with the discriminator).
public sealed record DefaultFilterDto(
    [property: JsonPropertyName("FilterType")] string FilterType,
    [property: JsonPropertyName("value")] IReadOnlyDictionary<string, object?> Value
)
{
    // Config records are self-wrapping, so the filter's own fields live on Value; the wrapper's FilterType and Value are skipped.
    public static DefaultFilterDto From(IFilterConfig config)
    {
        var configType = config.GetType();
        var filterType = configType.DeclaringType?.Name ?? configType.Name;
        var inner = config.GetValue();

        var fields = inner
            .GetType()
            .GetProperties()
            .Where(property => property.Name is not "FilterType" and not "Value")
            .ToDictionary(
                property => property.Name,
                property => property.GetValue(inner) is Enum value ? value.ToString() : property.GetValue(inner)
            );

        return new DefaultFilterDto(filterType, fields);
    }
}

// What each stage resolves for a null config: the database settings, or the code constants when a row is missing.
public sealed record PipelineDefaultsDto(
    IReadOnlyList<DefaultFilterDto> Cleanup,
    IReadOnlyList<DefaultFilterDto> ElevationProfile,
    IReadOnlyList<DefaultFilterDto> Peaks,
    float PeakProximityRadiusM,
    IReadOnlyList<DefaultFilterDto> Route
);
