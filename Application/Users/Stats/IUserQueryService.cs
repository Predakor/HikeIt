using Application.Commons.Interfaces;
using Application.Users.Dtos;
using Application.Users.RegionProgresses.Dtos;
using Domain.Common.Result;

namespace Application.Users.Stats;

public interface IUserQueryService : IQueryService {
    Task<Result<UserStatsDto.All>> GetStats(Guid userId);
    Task<Result<UserDto.Profile>> GetProfile(Guid userId);

    Task<Result<RegionProgressDto.Summary[]>> GetRegionsSummaries(Guid userId);
    Task<Result<RegionProgressDto.Full>> GetRegionProgess(Guid userId, int RegionId);
}
