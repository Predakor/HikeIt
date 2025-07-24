using Domain.Common.Result;
using Domain.Interfaces;
using Domain.Users.ValueObjects;

namespace Domain.Users;

public interface IUserRepository : IReadRepository<User, Guid> {
    Task<bool> Create(User newUser);
    Task<Result<bool>> UpdateStats(Guid userId, StatsUpdates.All update);
}
