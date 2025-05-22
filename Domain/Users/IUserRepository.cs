using Domain.Users;

namespace Infrastructure.Repository;

public interface IUserRepository {
    Task<User?> GetUser(int id);
    Task<List<User>> GetAllUsers();
    Task<bool> Create(User newUser);
}
