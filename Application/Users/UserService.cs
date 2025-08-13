using Application.Dto;
using Application.Services.Auth;
using Domain.Common;
using Domain.Common.Result;
using Domain.Users;

namespace Application.Users;

public class UserService(IUserRepository repository, IAuthService authService) : IUserService {
    readonly IUserRepository _repository = repository;
    readonly IAuthService _authService = authService;

    public async Task<Result<UserDto.Complete>> GetMe() {
        return await _authService
            .Me()
            .BindAsync(user => _repository.GetByIdAsync(user.Id))
            .MapAsync(UserDtoFactory.ToComplete);
    }

    public async Task<Result<UserDto.PublicProfile>> GetUserAsync(Guid id) {
        return await _repository.GetByIdAsync(id).MapAsync(UserDtoFactory.ToPublicProfile);
    }

    public async Task<Result<User>> CreateUserAsync(UserDto.Complete dto) {
        var user = new User {
            FirstName = "jausz",
            LastName = "bizensme",
            UserName = dto.UserName,
            Email = dto.Email,
        };

        user.SetBirthday(dto.BirthDay);

        if (await _repository.Create(user)) {
            return Errors.Unknown("couldn't save user");
        }

        return user;
    }
}
