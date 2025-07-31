using Domain.Common.Result;
using Domain.Interfaces;
using Domain.Users.Extentions;
using Domain.Users.RegionProgres;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public interface IUserRepository : ICrudResultRepository<User, Guid> {
    Task<bool> Create(User newUser);
    Task<Result<bool>> UpdateStats(
        Guid userId,
        StatsUpdates.All update,
        UpdateMode updateMode = UpdateMode.Increase
    );

    Task<Result<User>> GetWithRegionProgresses(Guid userId);

    Task<Result<RegionProgress>> CreateRegionProgress(RegionProgress regionProgres);
}
