using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Application.Trips.Analytics.Pipeline.Runners;

public interface IPointsFilterRunner
{
    List<GpxPoint> Apply(IEnumerable<GpxPoint> points, IEnumerable<IPointFilterConfig> configs);

    Task<Result<List<GpxPoint>>> ApplyAsync(
        IEnumerable<GpxPoint> points,
        StageId stage,
        IReadOnlyList<IPointFilterConfig>? configs = null,
        CancellationToken cancellationToken = default
    );
}

public sealed class PointsFilterRunner(IPipelineSettingsService settings) : IPointsFilterRunner
{
    /// <summary>Runs point filters on a copy of the points; the input list is never mutated.</summary>
    public List<GpxPoint> Apply(IEnumerable<GpxPoint> points, IEnumerable<IPointFilterConfig> configs)
    {
        var mutablePoints = points.Select(point => point.ToMutable()).ToList();

        foreach (var config in configs)
        {
            mutablePoints = config
                .CreateFilter()
                .Apply(mutablePoints)
                .ToList();
        }

        return mutablePoints
            .Select(point => point.ToGpxPoint())
            .ToList();
    }

    public async Task<Result<List<GpxPoint>>> ApplyAsync(
        IEnumerable<GpxPoint> points,
        StageId stage,
        IReadOnlyList<IPointFilterConfig>? configs = null,
        CancellationToken cancellationToken = default
    )
    {
        configs ??= await settings.GetFiltersAsync<IPointFilterConfig>(stage, cancellationToken);
        return Apply(points, configs);
    }
}