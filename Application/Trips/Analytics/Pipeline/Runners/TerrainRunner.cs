using Application.Trips.Analytics.Dtos;
using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Route;
using Domain.Trips.Analytics.Route.Filters;

namespace Application.Trips.Analytics.Pipeline.Runners;

public interface ITerrainRunner
{
    Task<Result<RouteAnalytic>> ApplyAsync(
        List<GpxPoint> points,
        List<GpxGain> gains,
        IReadOnlyList<ITerrainFilterConfig>? configs = null,
        List<PipeStageDto.StepTiming>? steps = null,
        CancellationToken cancellationToken = default
    );
}

public sealed class TerrainRunner(IPipelineSettingsService settings) : ITerrainRunner
{
    public async Task<Result<RouteAnalytic>> ApplyAsync(
        List<GpxPoint> points,
        List<GpxGain> gains,
        IReadOnlyList<ITerrainFilterConfig>? configs = null,
        List<PipeStageDto.StepTiming>? steps = null,
        CancellationToken cancellationToken = default
    )
    {
        configs ??= await settings.GetFiltersAsync<ITerrainFilterConfig>(StageId.Terrain, cancellationToken);

        return await FilterChain
            .RunAsync(new RouteAnalyticContext(points, gains), configs, steps)
            .MapAsync(context => context.ToAnalytic());
    }
}
