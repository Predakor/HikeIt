using Application.Dto;
using Domain.Entiites.Users;

namespace Application.Services.Users;

public class UserService(IUserRepository repository) : IUserService {
    readonly IUserRepository _repository = repository;

    public async Task<IEnumerable<UserDto.Complete>> GetAllUsersAsync() {
        var users = await _repository.GetAllAsync();
        return users.Select(user => new UserDto.Complete(user.Name, user.Email, user.BirthDay));
    }

    public async Task<UserDto.Complete?> GetUserByIdAsync(Guid id) {
        var user = await _repository.GetByIdAsync(id);
        if (user is null) {
            return null;
        }

        return new UserDto.Complete(user.Name, user.Email, user.BirthDay);
    }

    public async Task CreateUserAsync(UserDto.Complete dto) {
        var user = new User {
            Name = dto.Name,
            Email = dto.Email,
            BirthDay = dto.BirthDay,
        };

        await _repository.Create(user);
    }
}
