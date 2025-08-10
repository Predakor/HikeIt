using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Domain.Common.Factories;

public static class GeoFactory {
    const int Srid = 4326;

    static readonly GeometryFactory Factory = NtsGeometryServices.Instance.CreateGeometryFactory(
        srid: 4326
    );

    public static MultiPoint CreateMultiPoint(IEnumerable<Point> points) {
        return Factory.CreateMultiPoint([.. points]);
    }

    public static Point CreatePoint(double lon, double lat) {
        var coordinate = new Coordinate(lon, lat);
        var point = Factory.CreatePoint(coordinate);
        point.SRID = Srid;

        return point;
    }
}
