using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace Infrastructure.Peaks.Extentions;

internal static class GeoExtentions {
    public static Geometry FixGeographyOrientation(this Geometry geom) {
        return geom switch {
            Polygon poly => EnsureCCW(poly),
            MultiPolygon mp => new MultiPolygon(
                [.. mp.Geometries.Select(g => EnsureCCW((Polygon)g))]
            ),
            _ => geom, // Points, LineStrings, etc. don't need fixing
        };
    }

    static Polygon EnsureCCW(Polygon polygon) {
        var shell = polygon.Shell;
        if (!Orientation.IsCCW(shell.Coordinates)) {
            shell = (LinearRing)shell.Reverse();
        }

        var holes = polygon
            .Holes.Select(h => Orientation.IsCCW(h.Coordinates) ? (LinearRing)h.Reverse() : h)
            .ToArray();

        return new Polygon(shell, holes);
    }
}