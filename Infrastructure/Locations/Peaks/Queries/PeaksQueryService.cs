using System.Collections.Immutable;
using Application.Commons.Mappers.Implementations;
using Application.Locations.Peaks;
using Domain.Common.Geography;
using Domain.Common.Geography.Factories;
using Domain.Common.Geography.ValueObjects;
using Domain.Peaks;
using Domain.ReachedPeaks.Builders;
using Infrastructure.Commons.Databases;
using Infrastructure.Locations.Peaks.Extentions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Locations.Peaks.Queries;

internal static class Mapper
{
    public static PeakDto.Simple ToSimple(this Peak peak)
    {
        return new(peak.Height, peak.Name, peak.RegionID);
    }

    public static List<PeakDto.Simple> ToSimple(this IEnumerable<Peak> foundPeaks)
    {
        return [.. foundPeaks.Select(p => p.ToSimple())];
    }

    public static PeakDto.Complete ToComplete(this Peak peak)
    {
        return new(peak.Height, peak.Name, RegionMapper.MapToCompleteDto(peak.Region));
    }

    public static List<PeakDto.Complete> ToComplete(this IEnumerable<Peak> foundPeaks)
    {
        return [.. foundPeaks.Select(p => p.ToComplete())];
    }
}

public class PeaksQueryService : IPeaksQueryService
{
    private readonly TripDbContext _tripDbContext;
    private IQueryable<Peak> Peaks => _tripDbContext.Peaks.AsNoTracking();

    public PeaksQueryService(TripDbContext tripDbContext)
    {
        _tripDbContext = tripDbContext;
    }

    public async Task<Result<PeakDto.Complete>> GetByIdAsync(int id)
    {
        var peak = await Peaks
            .Include(x => x.Region)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (peak == null)
        {
            return Errors.NotFound("peak with id: " + id);
        }

        return peak.ToComplete();
    }

    public async Task<Result<IEnumerable<PeakDto.Complete>>> GetAllAsync()
    {
        var peaks = await Peaks
            .Include(p => p.Region)
            .ToListAsync();

        if (peaks.Count == 0)
        {
            return Errors.EmptyCollection(nameof(Peak));
        }

        return peaks.ToComplete();
    }

    public async Task<Result<Peak>> GetPeakWithinRadius(GpxPoint point, float radius)
    {
        var matchedPeaks = await Peaks.FirstOrDefaultAsync(p =>
            p.Location.IsWithinDistance(point.ToTopologyPoint(), radius)
        );

        if (matchedPeaks is null)
        {
            return Errors.NotFound("Peaks", "radius", radius);
        }

        return matchedPeaks;
    }

    public async Task<Result<List<Peak>>> GetPeaksWithinRadius(IEnumerable<GpxPoint> points, float radius)
    {
        var multipoint = GeoFactory.CreateMultiPoint(points.ToGpxPoints());

        var foundPeaks = await Peaks
            .Where(peak => multipoint.IsWithinDistance(peak.Location, radius))
            .Distinct()
            .ToListAsync();

        if (foundPeaks.Count == 0)
        {
            return Errors.NotFound("No peak was found within " + radius + " radius");
        }

        return foundPeaks;
    }

    public async Task<List<Peak>> GetPeaksInBounds(GeoBounds bounds)
    {
        var box = GeoFactory.CreateBox(bounds);

        return await Peaks
            .Where(peak => peak.Location.Intersects(box))
            .ToListAsync();
    }

    public async Task<Result<Peak>> GetPeakWithNameFromRegion(string name, int regionId)
    {
        var result = await Peaks
            .Where(p => p.RegionID == regionId)
            .FirstOrDefaultAsync(p => p.Name == name);

        if (result is null)
        {
            return Errors.NotFound("peak", name);
        }

        return result;
    }
}
