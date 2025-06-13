using Domain.Trips.ValueObjects;

namespace Domain.Trips.Builders.GpxDataBuilder;

internal class MutableGpxPoint(double lat, double lon, double ele, DateTime? time = null) {
    public double Lat = lat;
    public double Lon = lon;
    public double Ele = ele;
    public DateTime? Time = time;
}

internal class GpxDataBuilder(List<GpxPoint> points) {
    List<MutableGpxPoint> _gpxPoints = [.. points.Select(p => p.ToMutable())];

    public GpxDataBuilder ClampElevationSpikes(double maxSpike = .5d) {
        _gpxPoints.ClampSpikes(maxSpike);
        return this;
    }

    public GpxDataBuilder ApplyEmaSmoothing(float alpha = .4f) {
        _gpxPoints.SmoothWithEma(alpha);
        return this;
    }

    public GpxDataBuilder ApplyMedianFilter(int medianSize = 5) {
        _gpxPoints.MedianSmooth(medianSize);
        return this;
    }

    public GpxDataBuilder RoundElevation(int decimals = 1) {
        foreach (var point in _gpxPoints) {
            point.Ele = Math.Round(point.Ele, decimals);
        }

        return this;
    }

    public GpxDataBuilder DownSample(int amount = 5) {
        _gpxPoints = GpxBuilderMethods.DownSample(_gpxPoints, amount);
        return this;
    }

    public AnalyticData Build() {
        return new(_gpxPoints.Select(p => p.ToGpxPoint()).ToList());
    }
}

internal class Helpers {
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
