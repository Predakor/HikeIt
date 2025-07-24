using Application.Dto;
using Domain.Common.Result;
using Domain.Users;

namespace Application.Services.Users;

public interface IUserService {
    Task<Result<UserDto.Complete>> GetMe();
    Task<Result<UserDto.PublicProfile>> GetUserAsync(Guid id);
    Task<Result<User>> CreateUserAsync(UserDto.Complete dto);
}
