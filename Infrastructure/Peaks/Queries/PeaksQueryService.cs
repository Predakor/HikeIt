using Application.Dto;
using Application.Mappers.Implementations;
using Application.Mountains;
using Domain.Common;
using Domain.Common.Extentions;
using Domain.Common.Factories;
using Domain.Common.Result;
using Domain.Peaks;
using Domain.ReachedPeaks.Builders;
using Domain.Trips.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Peaks.Extentions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Infrastructure.Peaks.Queries;

public class PeaksQueryService : IPeaksQueryService {
    readonly TripDbContext _tripDbContext;
    const float ProximityPeakSerach = 10000f;
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
        var peaks = await Peaks.Include(p => p.Region).ToListAsync();

        if (peaks.Count == 0) {
            return Errors.EmptyCollection(nameof(Peak));
        }

        return peaks.ToComplete();
    }

    public async Task<Result<Peak>> GetPeakWithinRadius(GpxPoint point, float radius) {
        var matchedPeaks = await Peaks.FirstOrDefaultAsync(p =>
            p.Location.IsWithinDistance(point.ToTopologyPoint(), radius)
        );

        if (matchedPeaks is null) {
            return Errors.NotFound("Peaks", "radius", radius);
        }

        return matchedPeaks;
    }

    async Task<Result<Peak>> GetNearestPeakWithin(GpxPoint point, float radius) {
        var query = await Peaks
            .Where(p => p.Location.IsWithinDistance(point.ToTopologyPoint(), radius))
            .OrderBy(p => p.Location.Distance(point.ToTopologyPoint()))
            .FirstOrDefaultAsync();

        if (query is null) {
            return Errors.NotFound("Peaks", "radius", radius);
        }
        return query;
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

    public async Task<Result<List<ReachedPeakDataBuilder>>> GetPeaksWithinRadius(
        IEnumerable<ReachedPeakDataBuilder> points,
        float radius
    ) {
        var potentialPeaks = points.ToImmutableArray();
        if (potentialPeaks.Length == 0) {
            return Errors.NotFound("No peak was found within " + radius + " radius");
        }

        List<ReachedPeakDataBuilder> matchedPeaks = [];
        if (potentialPeaks.Length == 1) {
            var point = potentialPeaks[0];
            return await GetNearestPeakWithin(point.Location, radius)
                .MapAsync(peak => {
                    point.WithPeak(peak);
                    matchedPeaks.Add(point);
                    return matchedPeaks;
                });
        }

        var nearbyPeaks = await Peaks
            .Where(p =>
                p.Location.IsWithinDistance(
                    potentialPeaks[0].Location.ToTopologyPoint(),
                    ProximityPeakSerach
                )
            )
            .Distinct()
            .ToListAsync();

        if (nearbyPeaks.IsNullOrEmpty()) {
            return Errors.NotFound("No peak was found within " + radius + " radius");
        }

        Console.WriteLine("Detected nearby: " + nearbyPeaks.Count);
        Console.WriteLine("Searching from those with Treshold: " + radius);

        foreach (var potentialPeak in potentialPeaks) {
            Peak? nearestPeak = Helpers.GetNearestPeakWithin(radius, nearbyPeaks, potentialPeak);

            if (nearestPeak is not null) {
                potentialPeak.WithPeak(nearestPeak);
                matchedPeaks.Add(potentialPeak);
            }
        }

        if (matchedPeaks.IsNullOrEmpty()) {
            return Errors.NotFound("No peak was found within " + radius + " radius");
        }

        Console.WriteLine("total matched peaks " + matchedPeaks.Count);
        return matchedPeaks;
    }

    public async Task<Result<Peak>> GetPeakWithNameFromRegion(string name, int regionId) {
        var result = await Peaks.Where(p => p.RegionID == regionId).FirstOrDefaultAsync(p => p.Name == name);

        if (result is null) {
            return Errors.NotFound("peak", name);
        }

        return result;
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

static class Helpers {
    public static Peak? GetNearestPeakWithin(
        float radius,
        List<Peak> nearbyPeaks,
        ReachedPeakDataBuilder potentialPeak
    ) {
        return nearbyPeaks
            .Where(p => CalculateDistance(potentialPeak, p) <= radius)
            .OrderBy(p => CalculateDistance(potentialPeak, p))
            .FirstOrDefault();
    }

    static double CalculateDistance(ReachedPeakDataBuilder potentialPeak, Peak realPeak) {
        var distance = DistanceHelpers.Distance2D(
            realPeak.Location.ToGeoPoint(),
            potentialPeak.Location
        );
        Console.WriteLine($"Distance: {distance}");
        return distance;
    }
}
