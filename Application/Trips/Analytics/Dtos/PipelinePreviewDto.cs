namespace Application.Trips.Analytics.Dtos;

// Dictionaries are keyed by the stage's JSON name, e.g. "elevationProfile".
// Comparison = the same stages run for the request's target (current production settings, or the untouched file).
public sealed record PipelinePreviewDto(
    IReadOnlyDictionary<string, PipeStageDto.Result> Results,
    IReadOnlyDictionary<string, PipeStageDto.Result> Comparison
);
