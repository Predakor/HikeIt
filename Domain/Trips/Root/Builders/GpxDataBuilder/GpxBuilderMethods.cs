using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Root.Builders.GpxDataBuilder;

internal static class GpxBuilderMethods {
    public static GpxPoint ToGpxPoint(this MutableGpxPoint p) => new(p.Lat, p.Lon, p.Ele, p.Time);

    public static MutableGpxPoint ToMutable(this GpxPoint p) => new(p.Lat, p.Lon, p.Ele, p.Time);

    public static List<MutableGpxPoint> SmoothWithEma(
        this List<MutableGpxPoint> points,
        float alpha
    ) {
        var prevEma = points.First().Ele;

        for (int i = 1; i < points.Count; i++) {
            MutableGpxPoint current = points[i];
            prevEma = alpha * current.Ele + (1 - alpha) * prevEma;
            current.Ele = prevEma;
        }

        return points;
    }

    public static List<MutableGpxPoint> MedianSmooth(this List<MutableGpxPoint> data, int number) {
        int half = (int)MathF.Floor(number / 2);

        return data.Select(
                (point, i) => {
                    var median = GetMedian(data, i, half);
                    point.Ele = median.Ele;
                    return point;
                }
            )
            .ToList();
    }

    public static List<MutableGpxPoint> ClampSpikes(
        this List<MutableGpxPoint> points,
        double maxChange
    ) {
        var clampedPoints = new List<MutableGpxPoint> { points[0] };

        for (int i = 1; i < points.Count; i++) {
            MutableGpxPoint current = points[i];
            MutableGpxPoint prev = points[i - 1];

            double diff = current.Ele - prev.Ele;

            double clampedDiff = Math.Clamp(diff, -maxChange, maxChange);
            current.Ele = prev.Ele + clampedDiff;

            clampedPoints.Add(current);
        }
        return clampedPoints;
    }

    public static List<MutableGpxPoint> DownSample(this List<MutableGpxPoint> points, int amount) {
        return points.Where((p, i) => i % amount == 0).ToList();
    }

    static MutableGpxPoint GetMedian(List<MutableGpxPoint> data, int i, int half) {
        List<MutableGpxPoint> window = Helpers
            .GetArrayWindow(data, i, half)
            .OrderBy(p => p.Ele)
            .ToList();

        int center = (int)MathF.Floor(window.Count / 2);
        return window[center];
    }
}
