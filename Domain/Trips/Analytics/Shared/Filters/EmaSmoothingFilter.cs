using Domain.Common.Abstractions;
using Domain.Trips.Root.Builders.GpxDataBuilder;
namespace Domain.Trips.Analytics.Shared.Filters;

// Exponential moving average. Targets says what it smooths; Time, Pinned and untargeted values are left alone.
public sealed class EmaSmoothingFilter : IFilter<MutableGpxPoint>
{
    // Targets omitted from JSON means Elevation, which is what production smooths.
    public sealed record Config(double Alpha, PointFields Targets = PointFields.Elevation) : IPointFilterConfig
    {
        public IFilter<MutableGpxPoint> CreateFilter() => new EmaSmoothingFilter(this);
    }

    // Float on purpose: the production builder always smoothed with a float alpha.
    private readonly float _alpha;
    private readonly double _coordinateAlpha;
    private readonly bool _smoothsElevation;
    private readonly bool _smoothsLatitude;
    private readonly bool _smoothsLongitude;

    public EmaSmoothingFilter(Config config)
    {
        bool alphaInRange = config.Alpha > 0 && config.Alpha <= 1;
        if (!alphaInRange)
        {
            throw new ArgumentOutOfRangeException(nameof(config), config.Alpha, "Alpha must be in (0, 1].");
        }

        config.Targets.EnsureAny(nameof(config));

        _alpha = (float)config.Alpha;
        _coordinateAlpha = config.Alpha;
        _smoothsElevation = config.Targets.HasFlag(PointFields.Elevation);
        _smoothsLatitude = config.Targets.HasFlag(PointFields.Latitude);
        _smoothsLongitude = config.Targets.HasFlag(PointFields.Longitude);
    }

    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        if (values.Count < 2)
        {
            return values;
        }

        double prevEma = values[0].Ele;

        // values[i - 1] already holds its smoothed coordinates when values[i] is updated.
        for (int i = 1; i < values.Count; i++)
        {
            MutableGpxPoint current = values[i];
            MutableGpxPoint previous = values[i - 1];

            if (_smoothsElevation)
            {
                prevEma = (_alpha * current.Ele) + ((1 - _alpha) * prevEma);
                current.Ele = prevEma;
            }

            if (_smoothsLatitude)
            {
                current.Lat = (_coordinateAlpha * current.Lat) + ((1 - _coordinateAlpha) * previous.Lat);
            }

            if (_smoothsLongitude)
            {
                current.Lon = (_coordinateAlpha * current.Lon) + ((1 - _coordinateAlpha) * previous.Lon);
            }
        }

        return values;
    }
}