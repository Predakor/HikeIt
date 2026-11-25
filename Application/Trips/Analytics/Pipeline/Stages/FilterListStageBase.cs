using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;

namespace Application.Trips.Analytics.Pipeline.Stages;

public abstract class FilterListStageBase<TFilter> : IPipelineStage
    where TFilter : IFilterConfig
{
    public abstract StageId Id { get; }

    public virtual IReadOnlyList<StageId> DependsOn => [StageId.Cleanup];

    public virtual string? SkipReason(Stage.Context context)
    {
        return null;
    }

    public abstract Task<Result<Stage.Output>> RunAsync(
        Stage.Context context,
        PipelineConfig config,
        CancellationToken cancellationToken
    );

    protected abstract IReadOnlyList<TFilter>? RequestFilters(PipelineConfig config);
}
