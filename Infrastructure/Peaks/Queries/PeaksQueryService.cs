using Application.Dto;
using Application.Mappers.Implementations;
using Application.Peaks;
using Domain.Common;
using Domain.Common.Result;
using Domain.Entiites.Peaks;
using Domain.Trips.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Peaks.Extentions;
using Infrastructure.Peaks.Factories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Peaks.Queries;

public class PeaksQueryService : IPeaksQueryService {
    readonly TripDbContext _tripDbContext;
    IQueryable<Peak> Peaks => _tripDbContext.Peaks.AsNoTracking();

    public PeaksQueryService(TripDbContext tripDbContext) {
        _tripDbContext = tripDbContext;
    }

    public async Task<Result<PeakDto.Complete>> GetByIdAsync(int id) {
        var peak = await Peaks.Include(x => x.Region).FirstOrDefaultAsync(e => e.Id == id);

        if (peak == null) {
            return Errors.NotFound("peak with id: " + id);
        }

        return peak.ToComplete();
    }

    public async Task<Result<IEnumerable<PeakDto.Complete>>> GetAllAsync() {
        var peaks = await Peaks.ToListAsync();

        if (peaks.Count == 0) {
            return Errors.EmptyCollection(nameof(Peak));
        }

        return peaks.ToComplete();
    }

    public async Task<Result<Peak>> GetPeakWithinRadius(GpxPoint point, float radius) {
        var matchedPeaks = await Peaks.FirstOrDefaultAsync(p =>
            p.Location.IsWithinDistance(point.ToGpxPoint(), radius)
        );

        if (matchedPeaks == null) {
            return Errors.NotFound("Peaks");
        }

        return matchedPeaks;
    }

    public async Task<Result<List<Peak>>> GetPeaksWithinRadius(
        IEnumerable<GpxPoint> points,
        float radius
    ) {
        var multipoint = GeoFactory.CreateMultiPoint(points.ToGpxPoints());

        var foundPeaks = await Peaks
            .Where(peak => multipoint.IsWithinDistance(peak.Location, radius))
            .Distinct()
            .ToListAsync();

        if (foundPeaks.Count == 0) {
            return Errors.NotFound("No peak was found within " + radius + " radius");
        }

        return foundPeaks;
    }


}

static class Mapper {
    public static PeakDto.Simple ToSimple(this Peak peak) {
        return new(peak.Height, peak.Name, peak.RegionID);
    }

    public static PeakDto.Complete ToComplete(this Peak peak) {
        return new(peak.Height, peak.Name, RegionMapper.MapToCompleteDto(peak.Region));
    }

    public static List<PeakDto.Simple> ToSimple(this IEnumerable<Peak> foundPeaks) {
        return [.. foundPeaks.Select(p => p.ToSimple())];
    }

    public static List<PeakDto.Complete> ToComplete(this IEnumerable<Peak> foundPeaks) {
        return [.. foundPeaks.Select(p => p.ToComplete())];
    }
}
