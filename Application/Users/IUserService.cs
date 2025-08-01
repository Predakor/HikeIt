using Application.Dto;
using Domain.Common.Result;

namespace Application.Users;

public interface IUserService {
    Task<Result<UserDto.Complete>> GetMe();
    Task<Result<UserDto.PublicProfile>> GetUserAsync(Guid id);
}
