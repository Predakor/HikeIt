using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

public sealed class EmaSmoothingFilter : IFilter<MutableGpxPoint>
{
    public sealed record Config(double Alpha) : FilterConfigBase<Config>(default!), IPointFilterConfig;

    // Float on purpose: the production builder always smoothed with a float alpha.
    private readonly float _alpha;
    public EmaSmoothingFilter(Config config)
    {
        _alpha = (float)config.Alpha;
    }

    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        if (values.Count < 2)
        {
            return values;
        }

        double prevEma = values[0].Ele;

        for (int i = 1; i < values.Count; i++)
        {
            MutableGpxPoint current = values[i];
            prevEma = (_alpha * current.Ele) + ((1 - _alpha) * prevEma);
            current.Ele = prevEma;
        }

        return values;
    }
}
