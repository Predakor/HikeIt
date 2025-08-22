using Domain.Users.Root;

namespace Application.Commons.Services.Auth;

public interface IAuthService {
    Task<Result<User>> Me();
    Task<Result<User>> WithLoggedUser();
    Task<Result<Guid>> WithLoggedUserId();

    Task<Result<User>> GetByLoginOrEmail(string loginOrEmail);
}
