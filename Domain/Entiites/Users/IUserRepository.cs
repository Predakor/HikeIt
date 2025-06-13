using Domain.Interfaces;

namespace Domain.Entiites.Users;

public interface IUserRepository : IReadRepository<User, Guid> {
    Task<bool> Create(User newUser);
}
