using Application.Interfaces;
using Domain.Common.Result;

namespace Application.Users.Stats;

public interface IUserQueryService : IQueryService {
    Task<Result<UserStatsDto.All>> GetStats(Guid userId);
}
