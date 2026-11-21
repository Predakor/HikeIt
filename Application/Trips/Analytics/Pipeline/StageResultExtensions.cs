using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Core.Serialization;

namespace Application.Trips.Analytics.Pipeline;

internal static class StageResultExtensions
{
    public static PipeStageDto.Result ToResult(
        this Stage.Output output,
        StageId stage,
        PreviewSetup setup,
        double durationMs
    )
    {
        var view = StagePresenter.Present(stage, output, setup.Input);
        var steps = setup.IncludeSteps
            ? output.Steps
            : null;

        var telemetry = new PipeStageDto.Telemetry(durationMs, view.PointCount, view.Data.ToJsonByteCount(), steps);

        return new PipeStageDto.Result(stage, true, null, view.Data, telemetry);
    }

    public static PipeStageDto.Result ToFailed(this StageId stage, string error)
    {
        return new PipeStageDto.Result(stage, false, error, null, null);
    }
}
