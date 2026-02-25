using Application.Commons.Services.Auth;
using Application.Users.Root.Dtos;
using Domain.Users.Root;
using Domain.Users.Root.ValueObjects;

namespace Application.Users.Root;

public class UserService(IUserRepository repository, IAuthService authService) : IUserService
{
    private readonly IUserRepository _repository = repository;
    private readonly IAuthService _authService = authService;

    public async Task<Result<UserDto.Complete>> GetMe()
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _repository.GetByIdAsync(user.Id))
            .MapAsync(UserDtoFactory.ToComplete);
    }

    public async Task<Result<UserDataDto.PublicProfile>> GetUserAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id).MapAsync(u => u.ToPublicProfile());
    }

    public async Task<Result<bool>> UpdatePersonalInfo(User user, PersonalInfoUpdate update)
    {
        var updateUser = user.UpdatePersonalInfo(update);
        return await _repository.SaveChangesAsync(CancellationToken.None);
    }
}
