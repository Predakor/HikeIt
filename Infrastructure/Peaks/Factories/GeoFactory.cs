using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Infrastructure.Peaks.Factories;

internal static class GeoFactory {
    static readonly GeometryFactory Factory = NtsGeometryServices.Instance.CreateGeometryFactory(
        srid: 4326
    );

    public static MultiPoint CreateMultiPoint(IEnumerable<Point> points) {
        return Factory.CreateMultiPoint([.. points]);
    }
}

