using Application.Services.Peaks;
using Domain.Common;
using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Trips.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
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

    public Task<Result<Peak>> GetPeaksWithinRadius(List<IGeoPoint> points, float radius) {
        throw new NotImplementedException();
    }

    public async Task<Result<IList<Peak>>> GetPeaksWithinRadius(
        IReadOnlyList<Point> points,
        float radius
    ) {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var bufferedPoints = geometryFactory.CreateMultiPoint(points.ToArray()).Buffer(radius);

        // zero distance to buffered area means intersection
        var result = await DbSet
            .Where(p => p.Location.IsWithinDistance(bufferedPoints, 0))
            .ToListAsync();

        return result.Count > 0
            ? result
            : Errors.NotFound("No peak wasfound within" + radius + " radius");
    }

    public async Task<Result<Peak>> GetPeakWithinRadius(Point point, float radius) {
        var matchedPeaks = await DbSet.FirstAsync(p => p.Location.IsWithinDistance(point, radius));

        if (matchedPeaks == null) {
            return Errors.NotFound("Peaks");
        }

        return matchedPeaks;
    }
}
