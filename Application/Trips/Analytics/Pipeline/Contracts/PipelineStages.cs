using Application.Trips.Analytics.Dtos;
using System.Text.Json.Serialization;

namespace Application.Trips.Analytics.Pipeline.Contracts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StageId
{
    Cleanup,
    Terrain,
    Pace,
    ElevationProfile,
    PeakMatching,
    Route,
}

// One implementation per StageId. Stages are pure with respect to the database: they must never write.
public interface IPipelineStage
{
    StageId Id { get; }

    IReadOnlyList<StageId> DependsOn { get; }

    Task<Result<Stage.Output>> RunAsync(
        Stage.Context context,
        PipelineConfig config,
        CancellationToken cancellationToken
    );
}

public static class Stage
{
    // What a stage produced, plus how long each of its filters took. What the client sees is decided by StagePresenter.
    public sealed record Output(object Data, IReadOnlyList<PipeStageDto.StepTiming>? Steps = null);

    public sealed record Context(PipelineInput Input, IReadOnlyDictionary<StageId, Output> Upstream);
}