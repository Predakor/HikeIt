using Application.Dto;
using Application.Services.Auth;
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
        return await _repository.GetByIdAsync(id).MapAsync(u => u.ToPublicProfile());
    }
}
