using Domain.Trips.ValueObjects;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Infrastructure.Peaks.Extentions;

internal static class GpxPointExtentions {
    public static Point ToGpxPoint(this GpxPoint point) {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var gpxLocation = geometryFactory.CreatePoint(new Coordinate(point.Lon, point.Lat));
        return gpxLocation;
    }

    public static IReadOnlyList<Point> ToGpxPoints(this IEnumerable<GpxPoint> points) {
        return [.. points.Select(p => p.ToGpxPoint())];
    }
}
