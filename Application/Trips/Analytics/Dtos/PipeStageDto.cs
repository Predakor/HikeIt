using Application.Trips.Analytics.Pipeline.Contracts;

namespace Application.Trips.Analytics.Dtos;

public static class PipeStageDto
{
    // Telemetry is null on a failed stage.
    public sealed record Result(StageId Stage, bool Ok, string? Error, object? Data, Telemetry? Telemetry);

    // Measurements of one stage run. Steps (time per filter) only when the request asked for them.
    public sealed record Telemetry(
        double DurationMs,
        int? PointCount,
        int PayloadBytes,
        IReadOnlyList<StepTiming>? Steps = null
    );

    public sealed record StepTiming(string Name, double DurationMs);
}
