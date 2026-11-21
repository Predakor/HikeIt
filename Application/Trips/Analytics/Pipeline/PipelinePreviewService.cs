using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Core.Serialization;
using Core.Timing;

namespace Application.Trips.Analytics.Pipeline;

public interface IPipelinePreviewService
{
    Task<Result<PipelinePreviewDto>> PreviewAsync(
        Guid tripId,
        PipelinePreviewRequest request,
        CancellationToken cancellationToken
    );
}

public sealed class PipelinePreviewService(
    IPipelineSource source,
    IEnumerable<IPipelineStage> stages,
    PreviewCache cache
) : IPipelinePreviewService
{
    private readonly Dictionary<StageId, IPipelineStage> _stagesById = stages.ToDictionary(stage => stage.Id);

    public Task<Result<PipelinePreviewDto>> PreviewAsync(
        Guid tripId,
        PipelinePreviewRequest request,
        CancellationToken cancellationToken
    )
    {
        return source
            .LoadAsync(tripId, cancellationToken)
            .MapAsync(input => PreviewAsync(tripId, input, request, cancellationToken));
    }

    private async Task<PipelinePreviewDto> PreviewAsync(
        Guid tripId,
        PipelineInput input,
        PipelinePreviewRequest request,
        CancellationToken cancellationToken
    )
    {
        var targetStages = request.Stages ?? Enum.GetValues<StageId>();
        var requested = targetStages
            .Distinct()
            .ToList();

        var plan = new List<StageId>();

        foreach (var stage in requested)
        {
            AddWithDependencies(stage, plan);
        }

        var setup = new PreviewSetup(input, plan, requested, request.IncludeSteps);

        var preview = await RunSelectedAsync(setup, request.Config, cancellationToken);

        // The untouched file is cleanup with no filters; every other section stays at the production defaults.
        var comparisonConfig = request.Target switch
        {
            ComparisonTarget.Raw => new PipelineConfig(Cleanup: new PipeConfig.Cleanup([])),
            _ => new PipelineConfig(),
        };

        // Only the current pipe is recomputed on every edit; the comparison comes from the cache while the file and settings are unchanged.
        var comparison = await cache.GetOrRunAsync(
            request.Target,
            tripId,
            requested,
            setup.IncludeSteps,
            () => RunSelectedAsync(setup, comparisonConfig, cancellationToken),
            cancellationToken
        );

        return new PipelinePreviewDto(preview, comparison);
    }

    private async Task<IReadOnlyDictionary<string, PipeStageDto.Result>> RunSelectedAsync(
        PreviewSetup setup,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var runs = await RunPlanAsync(setup, config, cancellationToken);
        return setup.Requested.ToDictionary(
            stage => stage.ToJsonName(),
            stage => runs[stage].Result
        );
    }

    private async Task<Dictionary<StageId, StageRun>> RunPlanAsync(
        PreviewSetup setup,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var runs = new Dictionary<StageId, StageRun>();

        foreach (var stage in setup.Plan)
        {
            var failedUpstream = _stagesById[stage]
                .DependsOn
                .Where(dependency => !runs[dependency].Result.Ok)
                .Select(dependency => (StageId?)dependency)
                .FirstOrDefault();

            if (failedUpstream is StageId failed)
            {
                runs[stage] = new StageRun(stage.ToFailed($"Upstream stage {failed.ToJsonName()} failed"), null);
                continue;
            }

            runs[stage] = await RunStageAsync(stage, setup, config, runs, cancellationToken);
        }

        return runs;
    }

    private async Task<StageRun> RunStageAsync(
        StageId stage,
        PreviewSetup setup,
        PipelineConfig config,
        IReadOnlyDictionary<StageId, StageRun> upstreamRuns,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var handler = _stagesById[stage];
            var upstream = handler.DependsOn
                .ToDictionary(
                    dependency => dependency,
                    dependency => upstreamRuns[dependency].Output!
                );

            var context = new Stage.Context(setup.Input, upstream);
            var (elapsed, run) = await Timed.RunAsync(() => handler.RunAsync(context, config, cancellationToken));

            return run.Match(
                output => new StageRun(output.ToResult(stage, setup, elapsed.TotalMilliseconds), output),
                error => new StageRun(stage.ToFailed(error.Message), null)
            );
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            return new StageRun(stage.ToFailed(ex.Message), null);
        }
    }

    private void AddWithDependencies(StageId stage, List<StageId> plan)
    {
        if (plan.Contains(stage))
        {
            return;
        }

        foreach (var dependency in _stagesById[stage].DependsOn)
        {
            AddWithDependencies(dependency, plan);
        }

        plan.Add(stage);
    }

    private sealed record StageRun(PipeStageDto.Result Result, Stage.Output? Output);
}