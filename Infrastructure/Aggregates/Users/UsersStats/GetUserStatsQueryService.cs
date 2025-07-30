using Application.Users.Stats;
using Domain.Common;
using Domain.Common.Result;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Aggregates.Users.UsersStats;

internal class UserQueryService : IUserQueryService {
    readonly TripDbContext _dbContext;

    public UserQueryService(TripDbContext ctx) {
        _dbContext = ctx;
    }

    public async Task<Result<UserStatsDto.All>> GetStats(Guid userId) {
        var query = await _dbContext
            .Users.AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => u.Stats.ToUserStatsDto())
            .FirstOrDefaultAsync();

        if (query is null) {
            return Errors.NotFound("user", "id", userId);
        }

        return query;
    }
}
