using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

public sealed class RoundingPrecisionFilter : IFilter<MutableGpxPoint>
{
    public sealed record Config(int Decimals) : FilterConfigBase<Config>(default!), IPointFilterConfig;
    private readonly int _decimals;
    public RoundingPrecisionFilter(Config config)
    {
        _decimals = config.Decimals;
    }
    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        foreach (var point in values)
        {
            point.Ele = Math.Round(point.Ele, _decimals);
        }

        return values;
    }
}
