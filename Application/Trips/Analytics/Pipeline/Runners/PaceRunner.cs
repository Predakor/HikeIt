using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Route;
using Domain.Trips.Analytics.Time;
using Domain.Trips.Analytics.Time.Filters;

namespace Application.Trips.Analytics.Pipeline.Runners;

public interface IPaceRunner
{
    // Fails when the track has no usable timestamps.
    Task<Result<TimeAnalytic>> ApplyAsync(
        RouteAnalytic route,
        List<GpxPoint> points,
        IReadOnlyList<IPaceFilterConfig>? configs = null,
        List<PipeStageDto.StepTiming>? steps = null,
        CancellationToken cancellationToken = default
    );
}

public sealed class PaceRunner(IPipelineSettingsService settings) : IPaceRunner
{
    public async Task<Result<TimeAnalytic>> ApplyAsync(
        RouteAnalytic route,
        List<GpxPoint> points,
        IReadOnlyList<IPaceFilterConfig>? configs = null,
        List<PipeStageDto.StepTiming>? steps = null,
        CancellationToken cancellationToken = default
    )
    {
        configs ??= await settings.GetFiltersAsync<IPaceFilterConfig>(StageId.Pace, cancellationToken);

        return await TimeAnalyticContext
            .Create(route, points)
            .BindAsync(context => FilterChain.RunAsync(context, configs, steps))
            .MapAsync(context => context.ToAnalytic());
    }
}
