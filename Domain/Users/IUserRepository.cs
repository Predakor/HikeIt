namespace Domain.Users;

public interface IUserRepository : IReadRepository<User> {
    Task<bool> Create(User newUser);
}
