using Application.Interfaces;
using Application.Users.RegionProgresses.Dtos;
using Domain.Common.Result;

namespace Application.Users.Stats;

public interface IUserQueryService : IQueryService {
    Task<Result<UserStatsDto.All>> GetStats(Guid userId);
    Task<Result<RegionProgressDto.Summary[]>> GetRegionsSummaries(Guid userId);
}
