using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class CleanupStage(IPipelineSettingsService settings, IPointsFilterRunner runner)
    : FilterListStageBase<IPointFilterConfig>(settings)
{
    public override StageId Id => StageId.Cleanup;

    protected override IReadOnlyList<IPointFilterConfig>? RequestFilters(PipelineConfig config)
    {
        return config.Cleanup?.Filters;
    }

    public override Task<Result<StageOutput>> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var rawPoints = context.Input.RawPoints;

        return runner
            .ApplyAsync(rawPoints, Id, RequestFilters(config), cancellationToken)
            .MapAsync(cleanedPoints => ToOutput(rawPoints.Count, cleanedPoints));
    }

    private static StageOutput ToOutput(int rawCount, List<GpxPoint> cleanedPoints)
    {
        var data = new StageData.Cleanup(rawCount, cleanedPoints.Count);

        return new StageOutput(data, cleanedPoints.Count, new AnalyticData(cleanedPoints));
    }

    internal static AnalyticData CleanedDataFrom(StageContext context)
    {
        return context.Upstream[StageId.Cleanup].Internal as AnalyticData
            ?? throw new InvalidOperationException("Cleanup output is missing its cleaned points.");
    }
}
