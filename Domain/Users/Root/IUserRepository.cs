using Domain.Common.Abstractions;
using Domain.Users.RegionProgressions;
using Domain.Users.Stats;
using Domain.Users.Stats.Extentions;
using Domain.Users.Stats.ValueObjects;

namespace Domain.Users.Root;

public interface IUserRepository : ICrudResultRepository<User, Guid> {
    Task<bool> Create(User newUser);
    Task<Result<bool>> UpdateStats(
        Guid userId,
        UserStatsUpdates.All update,
        UpdateMode updateMode = UpdateMode.Increase
    );

    Task<Result<User>> GetWithRegionProgresses(Guid userId);
    Task<Result<UserStats>> GetUserStats(Guid userId);

    Task<Result<RegionProgress>> CreateRegionProgress(RegionProgress regionProgres);
}
