using Domain.Common.Geography.ValueObjects;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Domain.Common.Geography.Factories;

public static class GeoFactory
{
    private const int Srid = 4326;

    private static readonly GeometryFactory Factory = NtsGeometryServices.Instance.CreateGeometryFactory(
        srid: 4326
    );

    public static MultiPoint CreateMultiPoint(IEnumerable<Point> points)
    {
        return Factory.CreateMultiPoint([.. points]);
    }

    public static Polygon CreateBox(GeoBounds bounds)
    {
        var ring = Factory.CreateLinearRing(
            [
                new Coordinate(bounds.MinLon, bounds.MinLat),
                new Coordinate(bounds.MaxLon, bounds.MinLat),
                new Coordinate(bounds.MaxLon, bounds.MaxLat),
                new Coordinate(bounds.MinLon, bounds.MaxLat),
                new Coordinate(bounds.MinLon, bounds.MinLat),
            ]
        );

        return Factory.CreatePolygon(ring);
    }

    public static Point CreatePoint(double lon, double lat)
    {
        var coordinate = new Coordinate(lon, lat);
        var point = Factory.CreatePoint(coordinate);
        point.SRID = Srid;

        return point;
    }
}