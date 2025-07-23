using Domain.Common.Result;
using Domain.Users;

namespace Application.Services.Auth;
public interface IAuthService {
    Task<Result<User>> Me();
    Task<Result<User>> WithLoggedUser();
    Task<Result<User>> GetByLoginOrEmail(string loginOrEmail);

}

