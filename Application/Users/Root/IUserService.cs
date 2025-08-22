using Application.Users.Root.Dtos;
using Domain.Users.Root;
using Domain.Users.Root.ValueObjects;

namespace Application.Users.Root;

public interface IUserService {
    Task<Result<UserDto.Complete>> GetMe();
    Task<Result<UserDataDto.PublicProfile>> GetUserAsync(Guid id);
    Task<Result<bool>> UpdatePersonalInfo(User user, PersonalInfoUpdate update);
}
