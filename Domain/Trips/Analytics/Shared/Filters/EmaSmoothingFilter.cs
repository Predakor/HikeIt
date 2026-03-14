using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

public sealed class EmaSmoothingFilter : IFilter<MutableGpxPoint>
{
    public sealed record Config(double Alpha) : FilterConfigBase<double>("EmaSmoothingAlpha", Alpha);

    private readonly double _alpha;
    public EmaSmoothingFilter(Config config)
    {
        _alpha = config.Alpha;
    }

    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        var prevEma = values[0].Ele;

        foreach (var point in values.Skip(1))
        {
            prevEma = (_alpha * point.Ele) + ((1 - _alpha) * prevEma);
            point.Ele = prevEma;
        }

        return values;
    }

}
