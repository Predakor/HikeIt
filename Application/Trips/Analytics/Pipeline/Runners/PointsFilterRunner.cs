using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Application.Trips.Analytics.Pipeline.Runners;

public interface IPointsFilterRunner
{
    // Pass steps to collect per-filter timings; leave it null for a normal, untimed run.
    Task<Result<List<GpxPoint>>> ApplyAsync(
        IEnumerable<GpxPoint> points,
        StageId stage,
        IReadOnlyList<IPointFilterConfig>? configs = null,
        List<PipeStageDto.StepTiming>? steps = null,
        CancellationToken cancellationToken = default
    );
}

public sealed class PointsFilterRunner(IPipelineSettingsService settings) : IPointsFilterRunner
{
    // Runs on a copy of the points; the input list is never mutated.
    public async Task<Result<List<GpxPoint>>> ApplyAsync(
        IEnumerable<GpxPoint> points,
        StageId stage,
        IReadOnlyList<IPointFilterConfig>? configs = null,
        List<PipeStageDto.StepTiming>? steps = null,
        CancellationToken cancellationToken = default
    )
    {
        configs ??= await settings.GetFiltersAsync<IPointFilterConfig>(stage, cancellationToken);

        IList<MutableGpxPoint> mutablePoints = points
            .Select(point => point.ToMutable())
            .ToList();

        return await FilterChain
            .RunAsync(mutablePoints, configs, steps)
            .MapAsync(filtered =>
                filtered
                    .Select(point => point.ToGpxPoint())
                    .ToList()
            );
    }
}
