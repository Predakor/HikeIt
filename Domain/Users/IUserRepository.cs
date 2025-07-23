using Domain.Interfaces;

namespace Domain.Users;

public interface IUserRepository : IReadRepository<User, Guid> {
    Task<bool> Create(User newUser);
}
