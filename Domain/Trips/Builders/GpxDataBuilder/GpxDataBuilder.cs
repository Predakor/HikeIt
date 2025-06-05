using Domain.Trips.ValueObjects;

namespace Domain.Trips.Builders.GpxDataBuilder;

internal class MuttableGpxPoint(double Lat, double Lon, double Ele, DateTime? Time = null) {
    public static MuttableGpxPoint Create(GpxPoint point) {
        return new(point.Lat, point.Lon, point.Ele, point.Time);
    }

    public double Lat { get; set; } = Lat;
    public double Lon { get; set; } = Lon;
    public double Ele { get; set; } = Ele;
    public DateTime? Time { get; set; } = Time;
}

public class GpxDataBuilder {
    List<MuttableGpxPoint> _gpxPoints;

    public GpxDataBuilder(GpxAnalyticData data) {
        _gpxPoints = data.Data.Select(MuttableGpxPoint.Create).ToList();
    }

    public static GpxAnalyticData ProcessData(GpxAnalyticData data) {
        return new GpxDataBuilder(data)
            .ClampElevationSpikes()
            .ApplyMedianFilter()
            .ApplyEmaSmoothing()
            .Build();
    }

    public GpxDataBuilder ClampElevationSpikes(double maxSpike = .5d) {
        _gpxPoints = Methods.ClampSpikes(_gpxPoints, maxSpike);
        return this;
    }

    public GpxDataBuilder ApplyEmaSmoothing(float alpha = .4f) {
        _gpxPoints = Methods.SmoothWithEma(_gpxPoints, alpha);
        return this;
    }

    public GpxDataBuilder ApplyMedianFilter(int medianSize = 5) {
        _gpxPoints = Methods.MedianSmooth(_gpxPoints, medianSize);
        return this;
    }

    public GpxAnalyticData Build() {
        return new(_gpxPoints.Select(Helpers.CreateGpxPoint).ToList());
    }
}

internal static class Methods {
    public static List<MuttableGpxPoint> SmoothWithEma(List<MuttableGpxPoint> points, float alpha) {
        var prevEma = points.First().Ele;

        for (int i = 1; i < points.Count; i++) {
            MuttableGpxPoint current = points[i];
            prevEma = alpha * current.Ele + (1 - alpha) * prevEma;
            current.Ele = prevEma;
        }

        return points;
    }

    public static List<MuttableGpxPoint> MedianSmooth(List<MuttableGpxPoint> data, int number) {
        int half = (int)MathF.Floor(number / 2);

        return data.Select(
                (point, i) => {
                    List<MuttableGpxPoint> window = Helpers
                        .GetArrayWindow(data, i, half)
                        .OrderBy(p => p.Ele)
                        .ToList();

                    int center = (int)MathF.Floor(window.Count / 2);
                    var median = window[center];
                    point.Ele = median.Ele;
                    return point;
                }
            )
            .ToList();
    }

    public static List<MuttableGpxPoint> ClampSpikes(
        List<MuttableGpxPoint> points,
        double maxChange
    ) {
        var clampedPoints = new List<MuttableGpxPoint> { points[0] };

        for (int i = 1; i < points.Count; i++) {
            MuttableGpxPoint current = points[i];
            MuttableGpxPoint prev = points[i - 1];

            double diff = current.Ele - prev.Ele;

            double clampedDiff = Math.Clamp(diff, -maxChange, maxChange);
            current.Ele = prev.Ele + clampedDiff;

            clampedPoints.Add(current);
        }
        return clampedPoints;
    }
}

internal class Helpers {
    public static GpxPoint CreateGpxPoint(MuttableGpxPoint p) => new(p.Lat, p.Lon, p.Ele, p.Time);

    public static IEnumerable<T> GetArrayWindow<T>(List<T> data, int i, int half) {
        var (start, end) = GetWindowBounds(data, i, half);
        return GetWindow(data, start, end);
    }

    public static IEnumerable<T> GetWindow<T>(List<T> data, int start, int end) {
        return data.Skip(start).Take(end - start + 1);
    }

    public static (int, int) GetWindowBounds<T>(List<T> arr, int center, int size) {
        int start = Math.Max(center - size, 0);
        int end = Math.Min(center + size, arr.Count - 1);

        return (start, end);
    }
}
