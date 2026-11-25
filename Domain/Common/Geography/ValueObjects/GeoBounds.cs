namespace Domain.Common.Geography.ValueObjects;

public readonly record struct GeoBounds(double MinLat, double MinLon, double MaxLat, double MaxLon)
{
    private const double MetersPerDegreeLat = 111_320d;
    private const double EarthRadiusM = 6_371_000d;

    private const double MaxPaddingLatDeg = 89d;

    public static GeoBounds Around(IEnumerable<GpxPoint> points, double marginM)
    {
        var all = points.ToList();
        if (all.Count == 0)
        {
            throw new ArgumentException("At least one point is required.", nameof(points));
        }

        double minLat = all.Min(p => p.Lat);
        double maxLat = all.Max(p => p.Lat);
        double minLon = all.Min(p => p.Lon);
        double maxLon = all.Max(p => p.Lon);

        double widestLatDeg = Math.Min(
            Math.Max(Math.Abs(minLat), Math.Abs(maxLat)) + marginM / MetersPerDegreeLat,
            MaxPaddingLatDeg
        );
        double widestLatRad = widestLatDeg * Math.PI / 180d;
        double metersPerDegreeLon = MetersPerDegreeLat * Math.Cos(widestLatRad);

        double lonPadDeg = marginM / metersPerDegreeLon;
        double lonSpanM = ((maxLon - minLon) + 2 * lonPadDeg) * metersPerDegreeLon;

        // Geography polygon edges are great-circle arcs that sag toward the pole; pad for it.
        double sagM = lonSpanM * lonSpanM * Math.Tan(widestLatRad) / (8 * EarthRadiusM);
        double latPadDeg = (marginM + sagM) / MetersPerDegreeLat;

        return new GeoBounds(
            Math.Max(minLat - latPadDeg, -90d),
            Math.Max(minLon - lonPadDeg, -180d),
            Math.Min(maxLat + latPadDeg, 90d),
            Math.Min(maxLon + lonPadDeg, 180d)
        );
    }
}
