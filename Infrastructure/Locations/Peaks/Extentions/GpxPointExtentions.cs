using Domain.Common.Geography.ValueObjects;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Infrastructure.Locations.Peaks.Extentions;

internal static class GpxPointExtentions
{
    private static readonly GeometryFactory geometryFactory =
        NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

    public static Point ToTopologyPoint(this GpxPoint point)
    {
        return geometryFactory.CreatePoint(new Coordinate(point.Lon, point.Lat));
    }

    public static IGeoPoint ToGeoPoint(this Point point)
    {
        return new GpxPoint(point.Y, point.X, point.Z);
    }

    public static IGeoPoint ToGeoPoint(this Coordinate coordinates)
    {
        return new GpxPoint(coordinates.Y, coordinates.X, coordinates.Z);
    }

    public static IList<IGeoPoint> ToGeoPoints(this IEnumerable<Coordinate> coordinates)
    {
        return [.. coordinates.Select(c => c.ToGeoPoint())];
    }

    public static IReadOnlyList<Point> ToGpxPoints(this IEnumerable<GpxPoint> points)
    {
        return [.. points.Select(p => p.ToTopologyPoint())];
    }

    public static Coordinate ToCoordinate(this IGeoPoint point)
    {
        return new CoordinateZ(point.Lon, point.Lat, point.Ele);
    }

    public static IEnumerable<Coordinate> ToCoordinates(this IEnumerable<IGeoPoint> points)
    {
        return points.Select(p => p.ToCoordinate());
    }
}
