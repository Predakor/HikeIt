using Domain.Trips.Analytics.Shared.Filters;
using Domain.Common.Abstractions;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared;

public static class PointFilters
{
    private static readonly Dictionary<Type, Func<IFilterConfig, IFilter<MutableGpxPoint>>> Handlers = new()
    {
        [typeof(MedianSmoothingFilter.Config)] = Create<MedianSmoothingFilter.Config>(config => new MedianSmoothingFilter(config)),
        [typeof(MaxSpikeFilter.Config)] = Create<MaxSpikeFilter.Config>(config => new MaxSpikeFilter(config)),
        [typeof(EmaSmoothingFilter.Config)] = Create<EmaSmoothingFilter.Config>(config => new EmaSmoothingFilter(config)),
        [typeof(RoundingPrecisionFilter.Config)] = Create<RoundingPrecisionFilter.Config>(config => new RoundingPrecisionFilter(config)),
        [typeof(PinLocalMaximaFilter.Config)] = Create<PinLocalMaximaFilter.Config>(config => new PinLocalMaximaFilter(config)),
        [typeof(SimplifyFilter.Config)] = Create<SimplifyFilter.Config>(config => new SimplifyFilter(config)),
    };

    public static IFilter<MutableGpxPoint> CreateFilter(IFilterConfig config)
    {
        if (!Handlers.TryGetValue(config.GetType(), out var handler))
        {
            throw new NotSupportedException($"Filter type {config.GetType().Name} is not supported.");
        }

        return handler(config);
    }

    public static IList<IFilter<MutableGpxPoint>> CreateFilterList(IEnumerable<IFilterConfig> configs)
    {
        return [.. configs.Select(CreateFilter)];
    }

    /// <summary>Runs point filters on a copy of the points; the input list is never mutated.</summary>
    public static List<GpxPoint> Apply(IEnumerable<GpxPoint> points, IEnumerable<IFilterConfig> configs)
    {
        var mutablePoints = points.Select(point => point.ToMutable()).ToList();

        foreach (var filter in CreateFilterList(configs))
        {
            mutablePoints = [.. filter.Apply(mutablePoints)];
        }

        return mutablePoints.Select(point => point.ToGpxPoint()).ToList();
    }

    private static Func<IFilterConfig, IFilter<MutableGpxPoint>> Create<TConfig>(Func<TConfig, IFilter<MutableGpxPoint>> create)
        where TConfig : IFilterConfig
    {
        return config => create((TConfig)config);
    }
}
