using Application.Dto;
using Application.Mappers.Implementations;
using Application.Mountains;
using Domain.Common;
using Domain.Common.Result;
using Domain.Mountains.Peaks;
using Domain.Mountains.Regions;
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

    public async Task<Result<RegionDto.WithPeaks>> AllPeaksFromRegion(RegionDto.Complete region) {
        var peaksFromRegion = await Peaks.Where(p => p.RegionID == region.Id).ToListAsync();

        if (peaksFromRegion.Count == 0) {
            return Errors.EmptyCollection("No peaks found");
        }

        return region.ToRegionWithPeaks(peaksFromRegion);
    }


}

static class Mappers {
    public static RegionDto.WithPeaks ToRegionWithPeaks(
        this RegionDto.Complete region,
        List<Peak> peaks
    ) {
        var mappedPeaks = peaks.Select(p => new PeakDto.Base(p.Height, p.Name)).ToList();

        return new RegionDto.WithPeaks(region, mappedPeaks);
    }

    public static RegionDto.Complete ToComplete(this Region region) {
        return RegionMapper.MapToCompleteDto(region);
    }

    public static List<RegionDto.Complete> ToComplete(this IEnumerable<Region> regions) {
        return [.. regions.Select(ToComplete)];
    }
}
