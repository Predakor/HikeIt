using Application.Commons.Abstractions.Queries;
using Application.Users.RegionProgressions.Dtos;
using Application.Users.Root.Dtos;
using Application.Users.Stats.Dtos;

namespace Application.Users.Stats;

public interface IUserQueryService : IQueryService {
    Task<Result<UserStatsDto.All>> GetStats(Guid userId);
    Task<Result<UserDto.Profile>> GetProfile(Guid userId);

    Task<Result<RegionProgressDto.Summary[]>> GetRegionsSummaries(Guid userId);
    Task<Result<RegionProgressDto.Full>> GetRegionProgess(Guid userId, int RegionId);
}
