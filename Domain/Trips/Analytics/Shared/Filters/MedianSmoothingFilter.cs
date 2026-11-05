using Domain.Common.Abstractions;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

public sealed class MedianSmoothingFilter : IFilter<MutableGpxPoint>
{
    // Targets omitted from JSON means Elevation, which is what production smooths.
    public sealed record Config(int WindowSize, PointFields Targets = PointFields.Elevation) : IPointFilterConfig
    {
        public IFilter<MutableGpxPoint> CreateFilter() => new MedianSmoothingFilter(this);
    }

    private static readonly (PointFields Field, Func<MutableGpxPoint, double> Get, Action<MutableGpxPoint, double> Set)[] Accessors =
    [
        (PointFields.Elevation, p => p.Ele, (p, v) => p.Ele = v),
        (PointFields.Latitude, p => p.Lat, (p, v) => p.Lat = v),
        (PointFields.Longitude, p => p.Lon, (p, v) => p.Lon = v),
    ];

    private readonly int _half;
    private readonly PointFields _targets;

    public MedianSmoothingFilter(Config config)
    {
        config.Targets.EnsureAny(nameof(config));

        _half = config.WindowSize / 2;
        _targets = config.Targets;
    }

    // Centred window of up to 2 * half + 1 points, clipped at both ends. Values are overwritten in place,
    // so the left side of each window already holds smoothed values.
    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        foreach (var (field, get, set) in Accessors)
        {
            if (!_targets.HasFlag(field))
            {
                continue;
            }

            for (int i = 0; i < values.Count; i++)
            {
                set(values[i], GetMedian(values, i, _half, get));
            }
        }

        return values;
    }

    private static double GetMedian(IList<MutableGpxPoint> values, int i, int half, Func<MutableGpxPoint, double> get)
    {
        int start = Math.Max(i - half, 0);
        int end = Math.Min(i + half, values.Count - 1);

        List<double> window = values
            .Skip(start)
            .Take(end - start + 1)
            .Select(get)
            .Order()
            .ToList();

        return window[window.Count / 2];
    }
}