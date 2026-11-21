using Domain.AppSettings.Settings;
using Domain.Common.Abstractions;
using Domain.Trips.Root.Builders.GpxDataBuilder;

namespace Domain.Trips.Analytics.Shared.Filters;

// Ramer-Douglas-Peucker in local-plane coordinates (elevation as third axis). Never removes Pinned points.
public sealed class SimplifyFilter(SimplifyFilter.Config config) : SyncFilter<IList<MutableGpxPoint>>
{
    public sealed record Config(double ToleranceM) : IPointFilterConfig
    {
        public IFilter<IList<MutableGpxPoint>> CreateFilter() => new SimplifyFilter(this);
    }

    private const double EarthRadiusM = 6_371_000d;

    public override IList<MutableGpxPoint> Apply(IList<MutableGpxPoint> values)
    {
        if (values.Count < 3)
        {
            return values;
        }

        var planePoints = ToLocalPlane(values);
        var keep = new bool[values.Count];

        // Pinned points and the endpoints split the track into independent segments.
        var anchors = new List<int> { 0 };
        for (int i = 1; i < values.Count - 1; i++)
        {
            if (values[i].Pinned)
            {
                anchors.Add(i);
            }
        }
        anchors.Add(values.Count - 1);

        foreach (var anchor in anchors)
        {
            keep[anchor] = true;
        }

        for (int a = 0; a < anchors.Count - 1; a++)
        {
            KeepSignificantPoints(planePoints, anchors[a], anchors[a + 1], config.ToleranceM, keep);
        }

        return [.. values.Where((_, i) => keep[i])];
    }

    // Iterative Ramer-Douglas-Peucker over [first, last] so long tracks cannot overflow the stack.
    private static void KeepSignificantPoints(Point3[] points, int first, int last, double toleranceM, bool[] keep)
    {
        var segments = new Stack<(int First, int Last)>();
        segments.Push((first, last));

        while (segments.Count > 0)
        {
            var (start, end) = segments.Pop();
            if (end - start < 2)
            {
                continue;
            }

            var (farthestIndex, farthestDistance) = FindFarthest(points, start, end);
            if (farthestDistance <= toleranceM)
            {
                continue;
            }

            keep[farthestIndex] = true;
            segments.Push((start, farthestIndex));
            segments.Push((farthestIndex, end));
        }
    }

    private static (int Index, double Distance) FindFarthest(Point3[] points, int start, int end)
    {
        var farthestIndex = start;
        var farthestDistance = -1d;

        for (int i = start + 1; i < end; i++)
        {
            var distance = DistanceToSegment(points[i], points[start], points[end]);
            if (distance > farthestDistance)
            {
                farthestDistance = distance;
                farthestIndex = i;
            }
        }

        return (farthestIndex, farthestDistance);
    }

    private static double DistanceToSegment(Point3 p, Point3 a, Point3 b)
    {
        var (dx, dy, dz) = (b.X - a.X, b.Y - a.Y, b.Z - a.Z);
        var (px, py, pz) = (p.X - a.X, p.Y - a.Y, p.Z - a.Z);

        var lengthSquared = (dx * dx) + (dy * dy) + (dz * dz);
        var t = lengthSquared == 0 ? 0 : Math.Clamp(((px * dx) + (py * dy) + (pz * dz)) / lengthSquared, 0, 1);

        var (ex, ey, ez) = (px - (t * dx), py - (t * dy), pz - (t * dz));
        return Math.Sqrt((ex * ex) + (ey * ey) + (ez * ez));
    }

    // Equirectangular projection around the first point, elevation as the third axis.
    private static Point3[] ToLocalPlane(IList<MutableGpxPoint> values)
    {
        var origin = values[0];
        var metersPerDegreeLat = EarthRadiusM * Math.PI / 180d;
        var metersPerDegreeLon = metersPerDegreeLat * Math.Cos(origin.Lat * Math.PI / 180d);

        return [.. values.Select(p => new Point3(
            (p.Lon - origin.Lon) * metersPerDegreeLon,
            (p.Lat - origin.Lat) * metersPerDegreeLat,
            p.Ele
        ))];
    }

    private readonly record struct Point3(double X, double Y, double Z);
}