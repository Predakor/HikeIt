using Application.Commons.Abstractions;
using Domain.AppSettings.Settings;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

public sealed class MedianSmoothingFilter : IFilter<MutableGpxPoint>
{
    public sealed record Config(int WindowSize) : FilterConfigBase<Config>(default!), IPointFilterConfig;

    private readonly int _half;
    public MedianSmoothingFilter(Config config)
    {
        _half = config.WindowSize / 2;
    }

    // Centred window of up to 2 * half + 1 points, clipped at both ends. Elevations are overwritten in place,
    // so the left side of each window already holds smoothed values.
    public IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            values[i].Ele = GetMedianElevation(values, i, _half);
        }

        return values;
    }

    private static double GetMedianElevation(IList<MutableGpxPoint> values, int i, int half)
    {
        int start = Math.Max(i - half, 0);
        int end = Math.Min(i + half, values.Count - 1);

        List<MutableGpxPoint> window = values
            .Skip(start)
            .Take(end - start + 1)
            .OrderBy(p => p.Ele)
            .ToList();

        return window[window.Count / 2].Ele;
    }
}
