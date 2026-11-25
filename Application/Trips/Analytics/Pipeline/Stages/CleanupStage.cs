using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class CleanupStage(IPointsFilterRunner runner) : FilterListStageBase<IPointFilterConfig>
{
    public override StageId Id => StageId.Cleanup;

    public override IReadOnlyList<StageId> DependsOn => [];

    internal static AnalyticData CleanedDataFrom(Stage.Context context)
    {
        return context.Upstream[StageId.Cleanup].Data as AnalyticData
            ?? throw new InvalidOperationException("Cleanup output is missing its cleaned points.");
    }

    public override Task<Result<Stage.Output>> RunAsync(
        Stage.Context context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var rawPoints = context.Input.RawPoints;
        var steps = new List<PipeStageDto.StepTiming>();

        return runner
            .ApplyAsync(rawPoints, Id, RequestFilters(config), steps, cancellationToken)
            .MapAsync(cleaned => new Stage.Output(new AnalyticData(cleaned), steps));
    }

    protected override IReadOnlyList<IPointFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.Cleanup?.Filters;
    }
}
