using Application.Dto;
using Application.Mappers.Implementations;
using Application.Mountains;
using Domain.Common;
using Domain.Common.Result;
using Domain.Mountains.Regions;
using Domain.Peaks;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Aggregates.Mountains;

public class RegionQueryService : IRegionQueryService {
    readonly TripDbContext _context;

    IQueryable<Region> Regions => _context.Regions.AsNoTracking();
    IQueryable<Peak> Peaks => _context.Peaks.AsNoTracking();

    public RegionQueryService(TripDbContext context) {
        _context = context;
    }

    public async Task<Result<RegionDto.Complete>> GetByName(string name) {
        var region = await Regions.FirstOrDefaultAsync(r => r.Name == name);

        if (region is null) {
            return Errors.NotFound($"{nameof(Region)} with name: {name}");
        }

        return RegionMapper.MapToCompleteDto(region);
    }

    public async Task<Result<RegionDto.Complete>> GetById(int id) {
        var region = await Regions.FirstOrDefaultAsync(r => r.Id == id);

        if (region is null) {
            return Errors.NotFound($"{nameof(Region)} with id: {id}");
        }

        return RegionMapper.MapToCompleteDto(region);
    }

    public async Task<Result<List<RegionDto.Complete>>> GetAllAsync() {
        var regions = await Regions.ToListAsync();

        if (regions.Count == 0) {
            return Errors.EmptyCollection("no regions found");
        }

        return regions.ToComplete();
    }

    public async Task<Result<int>> GetPeakCount(int regionId) {
        return await Peaks.Where(r => r.RegionID == regionId).CountAsync();
    }

    public async Task<Result<RegionDto.WithPeaks>> AllPeaksFromRegion(RegionDto.Complete region) {
        var peaksFromRegion = await Peaks.Where(p => p.RegionID == region.Id).ToListAsync();

        if (peaksFromRegion.Count == 0) {
            return Errors.EmptyCollection("No peaks found");
        }

        return region.ToRegionWithPeaks(peaksFromRegion);
    }

    public async Task<Result<RegionDto.WithDetailedPeaks>> AllDetailedPeaksFromRegion(
        RegionDto.Complete region
    ) {
        var peaksFromRegion = await Peaks.Where(p => p.RegionID == region.Id).ToListAsync();

        if (peaksFromRegion.Count == 0) {
            return Errors.EmptyCollection("No peaks found");
        }

        return region.ToRegionWithDetailedPeaks(peaksFromRegion);
    }
}

static class Mappers {
    public static RegionDto.WithPeaks ToRegionWithPeaks(
        this RegionDto.Complete region,
        List<Peak> peaks
    ) {
        var mappedPeaks = peaks.Select(p => new PeakDto.Base(p.Height, p.Name, p.Id)).ToList();

        return new RegionDto.WithPeaks(region, mappedPeaks);
    }

    public static RegionDto.WithDetailedPeaks ToRegionWithDetailedPeaks(
        this RegionDto.Complete region,
        List<Peak> peaks
    ) {
        return new RegionDto.WithDetailedPeaks(region, peaks.ToPeaksWithLocation());
    }

    public static PeakDto.WithLocation ToWithLocation(this Peak peak) {
        return new(peak.Id, peak.Height, peak.Name, peak.Location.Y, peak.Location.X);
    }

    public static List<PeakDto.WithLocation> ToPeaksWithLocation(this IEnumerable<Peak> peaks) {
        return [.. peaks.Select(ToWithLocation)];
    }

    public static RegionDto.Complete ToComplete(this Region region) {
        return RegionMapper.MapToCompleteDto(region);
    }

    public static List<RegionDto.Complete> ToComplete(this IEnumerable<Region> regions) {
        return [.. regions.Select(ToComplete)];
    }
}
