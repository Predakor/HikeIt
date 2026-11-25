using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.Shared.Filters;

namespace Domain.Trips.Root.Builders.GpxDataBuilder;

public static class GpxBuilderMethods
{
    public static GpxPoint ToGpxPoint(this MutableGpxPoint p) => new(p.Lat, p.Lon, p.Ele, p.Time);

    public static MutableGpxPoint ToMutable(this GpxPoint p) => new(p.Lat, p.Lon, p.Ele, p.Time);

    public static List<MutableGpxPoint> SmoothWithEma(this List<MutableGpxPoint> points, float alpha)
    {
        new EmaSmoothingFilter(new EmaSmoothingFilter.Config(alpha)).Apply(points);

        return points;
    }

    public static List<MutableGpxPoint> MedianSmooth(this List<MutableGpxPoint> data, int number)
    {
        new MedianSmoothingFilter(new MedianSmoothingFilter.Config(number)).Apply(data);

        return data;
    }

    public static List<MutableGpxPoint> ClampSpikes(this List<MutableGpxPoint> points, double maxChange)
    {
        new MaxSpikeFilter(new MaxSpikeFilter.Config(maxChange)).Apply(points);

        return points;
    }

    public static List<MutableGpxPoint> DownSample(this List<MutableGpxPoint> points, int amount)
    {
        return points
            .Where((p, i) => i % amount == 0)
            .ToList();
    }
}
