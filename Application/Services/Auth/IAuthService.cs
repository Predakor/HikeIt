using Domain.Common.Result;
using Domain.Entiites.Users;

namespace Application.Services.Auth;
public interface IAuthService {
    Task<Result<User>> Me();
}