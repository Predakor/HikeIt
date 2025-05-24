using Application.Dto;

namespace Application.Services.Users;

public interface IUserService {
    Task<IEnumerable<UserDto.Complete>> GetAllUsersAsync();
    Task<UserDto.Complete?> GetUserByIdAsync(int id);
    Task CreateUserAsync(UserDto.Complete dto);
}
