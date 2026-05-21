using Application.Locations.Peaks;
using Application.Locations.Regions;
using Application.Users.RegionProgressions.Dtos;
using Application.Users.Regions;
using Domain.Trips.Analytics.Root;
using Infrastructure.Commons.Databases;
using Infrastructure.Commons.Extensions;
using Microsoft.EntityFrameworkCore;
using static Application.Users.Regions.IUserRegionsQueries;

namespace Infrastructure.Users.RegionProgressions.Queries;

internal class RegionProgressQueryService(TripDbContext dbContext) : IUserRegionsQueries
{
    public Task<Result<RoutePath[]>> GetRouteVisualizations(Guid userId, int regionId, CancellationToken ct)
    {
        return dbContext.Trips
            .Where(x => x.RegionId == regionId)
            .Where(x => x.UserId == userId)
            .Where(x => x.Analytics != null && x.Analytics.VisualisationPath != null)
            .Select(x => x.Analytics!.VisualisationPath!)
            .ToResultArrayAsync("visualizations", ct);
    }

    public Task<Result<IUserRegionsQueries.RegionTrip[]>> GetTrips(Guid userId, int regionId, CancellationToken ct)
    {
        return dbContext.Trips
            .Where(t => t.UserId == userId)
            .Where(t => t.RegionId == regionId)
            .Where(t => t.Analytics != null)
            .Include(t => t.Region)
            .Include(t => t.Analytics)
            .Select(t => new IUserRegionsQueries.RegionTrip(
                t.Id,
                t.Name,
                t.TripDay,
                (int?)t.Analytics!.RouteAnalytics!.TotalDistanceMeters,
                t.Analytics!.TimeAnalytics!.Duration)
            )
            .ToResultArrayAsync("trips", ct);
    }

    public async Task<Result<RegionData>> GetRegionProgess(Guid userId, int RegionId, CancellationToken ct)
    {
        var region = await dbContext.Regions
            .AsNoTracking()
            .Where(x => x.Id == RegionId)
            .Include(x => x.Peaks)
            .FirstOrDefaultAsync(ct);

        if (region is null)
        {
            return Errors.NotFound("region");
        }

        var regionPeakIdList = region.Peaks.Select(x => x.Id).ToList();

        var userPeaksFromRegion = await dbContext.ReachedPeaks
            .Include(x => x.Trip)
            .Where(x => x.UserId == userId)
            .Where(x => regionPeakIdList.Contains(x.PeakId))
            .Select(x => new { x.PeakId, x.TripId })
            .ToListAsync(ct);

        var userPeakIdList = userPeaksFromRegion.Select(x => x.PeakId).ToList();
        var userTripIdList = userPeaksFromRegion.Select(x => x.TripId).ToList();

        var highestPeak = region.Peaks.MaxBy(p => p.Height)!;

        var regionDto = new RegionDto.Complete(region.Id, region.Name);
        var regionProgessData = new RegionProgressDto.Full(
            regionDto,
            region.Peaks.Count,
            userPeaksFromRegion.Count,
            userPeaksFromRegion.Distinct().Count(),
            new(highestPeak.Height, highestPeak.Name, highestPeak.Id),
            region.Peaks.Select(x => new PeakDto.WithReachStatus(
                x.Id,
                x.Name,
                x.Height,
                userPeakIdList.Contains(x.Id)))
            .ToArray()
        );

        var hasTrips = userPeakIdList.Count != 0;
        var linkedDataState = hasTrips && await dbContext.TripAnalytics
            .Where(x => userTripIdList.Contains(x.Id))
            .AnyAsync(x => x.VisualisationPath != null, ct);

        return new RegionData(
            Progress: regionProgessData,
            HasTrips: hasTrips,
            HasVisualizations: linkedDataState
        );

    }
}
