using Application.Users.Dtos;
using Domain.Common.Result;
using Domain.Users;
using Domain.Users.ValueObjects;

namespace Application.Users;

public interface IUserService {
    Task<Result<UserDto.Complete>> GetMe();
    Task<Result<UserDataDto.PublicProfile>> GetUserAsync(Guid id);
    Task<Result<bool>> UpdatePersonalInfo(User user, PersonalInfoUpdate update);
}
