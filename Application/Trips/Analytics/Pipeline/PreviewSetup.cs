using Application.Trips.Analytics.Pipeline.Contracts;

namespace Application.Trips.Analytics.Pipeline;

// What stays the same for every run of one request; only the config differs between the current pipe and the comparison.
internal sealed record PreviewSetup(
    PipelineInput Input,
    IReadOnlyList<StageId> Plan,
    IReadOnlyList<StageId> Requested,
    bool IncludeSteps
);
