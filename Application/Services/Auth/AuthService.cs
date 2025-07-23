using Domain.Common;
using Domain.Common.Result;
using Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Auth;

public class AuthService : IAuthService {
    readonly UserManager<User> _userManager;
    readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager) {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<Result<User>> Me() {
        var cookieUser = _httpContextAccessor.HttpContext?.User;
        if (cookieUser is null) {
            return Errors.NotAuthorized();
        }

        var user = await _userManager.GetUserAsync(cookieUser);
        if (user is null) {
            return Errors.NotAuthorized();
        }

        return user;

    }

    //added for naming clarity
    public async Task<Result<User>> WithLoggedUser() {
        var cookieUser = _httpContextAccessor.HttpContext?.User;
        if (cookieUser is null) {
            return Errors.NotAuthorized();
        }

        var user = await _userManager.GetUserAsync(cookieUser);
        if (user is null) {
            return Errors.NotAuthorized();
        }

        return user;
    }



    public async Task<Result<User>> GetByLoginOrEmail(string loginOrEmail) {
        bool isEmail = loginOrEmail.Contains('@');

        var query = isEmail
            ? _userManager.FindByEmailAsync(loginOrEmail)
            : _userManager.FindByNameAsync(loginOrEmail);

        var user = await query;
        if (user == null) {
            var credentialType = isEmail ? "email" : "username";
            return Errors.NotFound(
                $"user with {credentialType}: {loginOrEmail}"
            );
        }

        return user;
    }


}
