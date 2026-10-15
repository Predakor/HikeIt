using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

// Opt-in exponential moving average over Lat/Lon to damp GPS drift. Not part of the production defaults.
public sealed class CoordinateSmoothingFilter : IFilter<MutableGpxPoint>
{
    public sealed record Config(double Alpha) : FilterConfigBase<Config>(default!), IPointFilterConfig;

    private readonly double _alpha;

    public CoordinateSmoothingFilter(Config config)
    {
        if (config.Alpha <= 0 || config.Alpha > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(config), config.Alpha, "Alpha must be in (0, 1].");
        }

        _alpha = config.Alpha;
    }

    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        if (values.Count < 2)
        {
            return values;
        }

        // values[i - 1] already holds its smoothed coordinates when values[i] is updated.
        for (int i = 1; i < values.Count; i++)
        {
            MutableGpxPoint current = values[i];
            MutableGpxPoint previous = values[i - 1];

            current.Lat = (_alpha * current.Lat) + ((1 - _alpha) * previous.Lat);
            current.Lon = (_alpha * current.Lon) + ((1 - _alpha) * previous.Lon);
        }

        return values;
    }
}
