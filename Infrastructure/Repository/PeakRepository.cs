using Application.Services.Peaks;
using Domain.Common;
using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace Infrastructure.Repository;

public class PeakRepository : Repository<Peak, int>, IPeakRepository {
    public PeakRepository(TripDbContext context)
        : base(context) { }

    public override async Task<IEnumerable<Peak>> GetAllAsync() {
        return await DbSet.Include(x => x.Region).ToListAsync();
    }

    public override async Task<Peak?> GetByIdAsync(int id) {
        return await DbSet.Include(x => x.Region).FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Result<IList<Peak>>> GetPeaksWithinRadius(
        IReadOnlyList<Point> points,
        float radius
    ) {
        if (points == null || points.Count == 0)
            return Errors.BadRequest("At least one point must be provided");

        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        var multipoint = geometryFactory.CreateMultiPoint([.. points]);

        var foundPeaks = await DbSet
            .Where(peak => multipoint.IsWithinDistance(peak.Location, radius))
            .ToListAsync();

        return foundPeaks.Count > 0
            ? foundPeaks
            : Errors.NotFound("No peak was found within " + radius + " radius");
    }

    public async Task<Result<Peak>> GetPeakWithinRadius(Point point, float radius) {
        var matchedPeaks = await DbSet.FirstAsync(p => p.Location.IsWithinDistance(point, radius));

        if (matchedPeaks == null) {
            return Errors.NotFound("Peaks");
        }

        return matchedPeaks;
    }
}

internal static class GeometryExtentions {
    public static Geometry FixGeographyOrientation(this Geometry geom) {
        return geom switch {
            Polygon poly => EnsureCCW(poly),
            MultiPolygon mp => new MultiPolygon(
                mp.Geometries.Select(g => EnsureCCW((Polygon)g)).ToArray()
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
