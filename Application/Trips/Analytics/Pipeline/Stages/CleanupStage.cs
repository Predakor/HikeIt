using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Shared;

namespace Application.Trips.Analytics.Pipeline.Stages;

public sealed class CleanupStage(IPipelineSettingsService settings) : FilterListStageBase<IPointFilterConfig>(settings)
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
        return ResolveFiltersAsync(config, cancellationToken)
            .MapAsync(filters => Clean(context.Input.RawPoints, filters));
    }

    private static StageOutput Clean(IReadOnlyList<GpxPoint> rawPoints, IEnumerable<IFilterConfig> filters)
    {
        var cleanedPoints = PointFilters.Apply(rawPoints, filters);
        var data = new StageData.Cleanup(rawPoints.Count, cleanedPoints.Count);

        return new StageOutput(data, cleanedPoints.Count, new AnalyticData(cleanedPoints));
    }

    internal static AnalyticData CleanedDataFrom(StageContext context)
    {
        return context.Upstream[StageId.Cleanup].Internal as AnalyticData
            ?? throw new InvalidOperationException("Cleanup output is missing its cleaned points.");
    }
}
