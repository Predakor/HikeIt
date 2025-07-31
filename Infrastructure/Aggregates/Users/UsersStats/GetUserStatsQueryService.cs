using Application.Users.RegionProgresses.Dtos;
using Application.Users.Stats;
using Domain.Common;
using Domain.Common.Result;
using Domain.Common.Utils;
using Domain.Users.RegionProgresses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Aggregates.Users.UsersStats;

internal class UserQueryService : IUserQueryService {
    readonly TripDbContext _dbContext;
    IQueryable<Domain.Users.User> Users => _dbContext.Users.AsNoTracking();

    public UserQueryService(TripDbContext ctx) {
        _dbContext = ctx;
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

    public async Task<Result<RegionProgressDto.Summary[]>> GetRegionsSummaries(Guid userId) {
        var query = await _dbContext
            .Set<RegionProgress>()
            .AsNoTracking()
            .Include(rp => rp.Region)
            .Where(rp => rp.UserId == userId)
            .Select(rp => rp.ToRegionSummary())
            .ToArrayAsync();

        if (query.NullOrEmpty()) {
            return Errors.EmptyCollection("region progresses");
        }

        return query;
    }

}

static class Extentions {
    public static RegionProgressDto.Summary ToRegionSummary(this RegionProgress progress) {
        return new RegionProgressDto.Summary(
            new(progress.Region.Id, progress.Region.Name),
            progress.TotalReachedPeaks,
            progress.TotalPeaksInRegion
        );
    }
}
