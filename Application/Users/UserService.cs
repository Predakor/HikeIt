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
            .MapAsync(user => _repository.GetByIdAsync(user.Id))
            .MapAsync(user => UserDtoFactory.CreateComplete(user));
    }

    public async Task<Result<UserDto.PublicProfile>> GetUserAsync(Guid id) {
        var user = await _repository.GetByIdAsync(id);
        if (user is null) {
            return Errors.NotFound("user");
        }

        return UserDtoFactory.CreatePublicProfile(user);
    }

    public async Task<Result<User>> CreateUserAsync(UserDto.Complete dto) {
        var user = new User {
            FirstName = "jausz",
            LastName = "bizensme",
            UserName = dto.UserName,
            Email = dto.Email,
            BirthDay = dto.BirthDay,
        };

        if (await _repository.Create(user)) {
            return Errors.Unknown("couldn't save user");
        }

        return user;
    }
}
