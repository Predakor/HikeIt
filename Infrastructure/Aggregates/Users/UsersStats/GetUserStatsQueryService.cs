using Application.Dto;
using Application.Mountains;
using Application.Users.RegionProgresses.Dtos;
using Application.Users.Stats;
using Domain.Common;
using Domain.Common.Result;
using Domain.Common.Utils;
using Domain.Users;
using Domain.Users.RegionProgresses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Aggregates.Users.UsersStats;

internal class UserQueryService : IUserQueryService {
    readonly TripDbContext _dbContext;
    readonly IRegionQueryService _regionQueries;
    IQueryable<User> Users => _dbContext.Users.AsNoTracking();
    IQueryable<RegionProgress> RegionProgresses => _dbContext.Set<RegionProgress>().AsNoTracking();

    public UserQueryService(TripDbContext ctx, IRegionQueryService regionQueries) {
        _dbContext = ctx;
        _regionQueries = regionQueries;
    }

    public async Task<Result<UserStatsDto.All>> GetStats(Guid userId) {
        var query = await Users
            .Where(u => u.Id == userId)
            .Select(u => u.Stats.ToUserStatsDto())
            .FirstOrDefaultAsync();

        if (query is null) {
            return Errors.NotFound("user", "id", userId);
        }

        return query;
    }

    public async Task<Result<UserDto.Profile>> GetProfile(Guid userId) {
        var query = await Users
            .Include(u => u.Stats)
            .Include(u => u.Rank)
            .Where(u => u.Id == userId)
            .Select(u => u.ToProfile())
            .FirstOrDefaultAsync();

        if (query is null) {
            return Errors.NotFound("user", userId);
        }

        return query;
    }

    public async Task<Result<RegionProgressDto.Summary[]>> GetRegionsSummaries(Guid userId) {
        var query = await RegionProgresses
            .Include(rp => rp.Region)
            .Where(rp => rp.UserId == userId)
            .Select(rp => rp.ToRegionSummary())
            .ToArrayAsync();

        if (query.NullOrEmpty()) {
            return Errors.EmptyCollection("region progresses");
        }

        return query;
    }

    public async Task<Result<RegionProgressDto.Full>> GetRegionProgess(Guid userId, int RegionId) {
        var region = (await _regionQueries.AllPeaksFromRegion(new(RegionId, ""))).Value;

        var regionSummary = await RegionProgresses
            .Include(rp => rp.Region)
            .Where(rp => rp.UserId == userId && rp.RegionId == RegionId)
            .FirstOrDefaultAsync();

        if (region is null) {
            return Errors.NotFound("region");
        }

        var peaksWithReachStatus = region
            .Peaks.Select(p => p.ToPeakDtoWithReachStatus(p.WasReached(regionSummary)))
            .ToArray();

        var highestPeak = region.Peaks.MaxBy(p => p.Height)!;

        var regionDto = new RegionDto.Complete(regionSummary.Region.Id, regionSummary.Region.Name);

        return new RegionProgressDto.Full(
            regionDto,
            regionSummary.TotalPeaksInRegion,
            regionSummary.TotalReachedPeaks,
            regionSummary.UniqueReachedPeaks,
            new(highestPeak.Height, highestPeak.Name, highestPeak.Id),
            peaksWithReachStatus
        );
    }
}

static class Extentions {
    public static UserDto.Profile ToProfile(this User user) {
        return new(user.ToPublicProfile(), user.ToPersonal(), user.ToAccountState());
    }

    public static RegionProgressDto.Summary ToRegionSummary(this RegionProgress progress) {
        return new RegionProgressDto.Summary(
            new(progress.Region.Id, progress.Region.Name),
            progress.UniqueReachedPeaks,
            progress.TotalPeaksInRegion
        );
    }

    public static bool WasReached(this PeakDto.Base peak, RegionProgress? regionSummary) {
        if (regionSummary is null) {
            return false;
        }

        return regionSummary.PeakVisits.ContainsKey(peak.Id);
    }

    public static PeakDto.WithReachStatus ToPeakDtoWithReachStatus(
        this PeakDto.Base peak,
        bool reached
    ) {
        return new(peak.Id, peak.Name, peak.Height, reached);
    }
}
