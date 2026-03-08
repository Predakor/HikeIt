using Application.Users.Root.Dtos;
using Domain.Users.Root;

namespace Application.Commons.Services.Auth;

public interface IAuthService
{
    Task<Result<User>> WithLoggedUser();
    Task<Result<Guid>> WithLoggedUserId();

    Task<Result<User>> GetByLoginOrEmail(string loginOrEmail);
    Task<Result<bool>> ResetPassword(UserDto.ResetPassword request);
    Task<Result<string>> ForgotPassword(string email);

}
