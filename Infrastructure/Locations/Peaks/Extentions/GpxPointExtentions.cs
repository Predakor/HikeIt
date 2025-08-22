using Domain.Common.Geography.ValueObjects;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Infrastructure.Locations.Peaks.Extentions;

internal static class GpxPointExtentions {
    static readonly GeometryFactory geometryFactory =
        NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

    public static Point ToTopologyPoint(this GpxPoint point) {
        return geometryFactory.CreatePoint(new Coordinate(point.Lon, point.Lat));
    }

    public static IGeoPoint ToGeoPoint(this Point point) {
        return new GpxPoint(point.Y, point.X, point.Z);
    }

    public static IReadOnlyList<Point> ToGpxPoints(this IEnumerable<GpxPoint> points) {
        return [.. points.Select(p => p.ToTopologyPoint())];
    }
}
