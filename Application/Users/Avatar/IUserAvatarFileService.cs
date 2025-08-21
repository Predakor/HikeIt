using Domain.Common.Result;
using Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Avatar;
public interface IUserAvatarFileService {
    Task<Result<string>> Upload(IFormFile file, User user);
    Task<Result<bool>> Delete(User user);
}
