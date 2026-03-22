using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Root.Builders.GpxDataBuilder;

public static class GpxBuilderMethods
{
    public static GpxPoint ToGpxPoint(this MutableGpxPoint p) => new(p.Lat, p.Lon, p.Ele, p.Time);

    public static MutableGpxPoint ToMutable(this GpxPoint p) => new(p.Lat, p.Lon, p.Ele, p.Time);

    public static List<MutableGpxPoint> SmoothWithEma(
        this List<MutableGpxPoint> points,
        float alpha
    )
    {
        var prevEma = points.First().Ele;

        for (int i = 1; i < points.Count; i++)
        {
            MutableGpxPoint current = points[i];
            prevEma = (alpha * current.Ele) + ((1 - alpha) * prevEma);
            current.Ele = prevEma;
        }

        return points;
    }

    public static List<MutableGpxPoint> MedianSmooth(this List<MutableGpxPoint> data, int number)
    {
        int half = number / 2;

        for (int i = 0; i < data.Count; i++)
        {
            data[i].Ele = GetMedianElevation(data, i, half);
        }

        return data;
    }

    public static List<MutableGpxPoint> ClampSpikes(
        this List<MutableGpxPoint> points,
        double maxChange
    )
    {
        var clampedPoints = new List<MutableGpxPoint> { points[0] };

        for (int i = 1; i < points.Count; i++)
        {
            MutableGpxPoint current = points[i];
            MutableGpxPoint prev = points[i - 1];

            double diff = current.Ele - prev.Ele;

            double clampedDiff = Math.Clamp(diff, -maxChange, maxChange);
            current.Ele = prev.Ele + clampedDiff;

            clampedPoints.Add(current);
        }
        return clampedPoints;
    }

    public static List<MutableGpxPoint> DownSample(this List<MutableGpxPoint> points, int amount)
    {
        return points.Where((p, i) => i % amount == 0).ToList();
    }

    private static double GetMedianElevation(List<MutableGpxPoint> data, int i, int half)
    {
        List<MutableGpxPoint> window = Helpers
            .GetArrayWindow(data, i, half)
            .OrderBy(p => p.Ele)
            .ToList();

        return window[window.Count / 2].Ele;
    }
}
