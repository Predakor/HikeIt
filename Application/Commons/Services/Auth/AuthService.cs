using Application.Users.Root.Dtos;
using Domain.Users.Root;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commons.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AuthService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ILogger<AuthService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _logger = logger;
    }

    //added for naming clarity
    public async Task<Result<User>> WithLoggedUser()
    {
        var cookieUser = _httpContextAccessor.HttpContext?.User;
        if (cookieUser is null)
        {
            return Errors.NotAuthorized();
        }

        var user = await _userManager.GetUserAsync(cookieUser);
        if (user is null)
        {
            return Errors.NotAuthorized();
        }

        return user;
    }

    public async Task<Result<Guid>> WithLoggedUserId()
    {
        var cookieUser = _httpContextAccessor.HttpContext?.User;
        if (cookieUser is null)
        {
            return Errors.NotAuthorized();
        }

        var user = await _userManager.GetUserAsync(cookieUser);
        if (user is null)
        {
            return Errors.NotAuthorized();
        }

        return user.Id;
    }

    public async Task<Result<User>> GetByLoginOrEmail(string loginOrEmail)
    {
        bool isEmail = loginOrEmail.Contains('@');

        var query = isEmail
            ? _userManager.FindByEmailAsync(loginOrEmail)
            : _userManager.FindByNameAsync(loginOrEmail);

        var user = await query;
        if (user == null)
        {
            var credentialType = isEmail ? "email" : "username";
            return Errors.NotFound($"user with {credentialType}: {loginOrEmail}");
        }

        return user;
    }

    public async Task<Result<bool>> ResetPassword(UserDto.ResetPassword request)
    {
        if (request.UserName is null)
        {
            return Errors.BadRequest("email can't be null");
        }

        if (request.Password is null)
        {
            return Errors.BadRequest("password can't be null");
        }

        return await GetByLoginOrEmail(request.UserName)
            .MapAsync(u => _userManager.ResetPasswordAsync(u, request.Token, request.Password))
            .MapAsync(r => r.Succeeded);

    }

    public async Task<Result<string>> ForgotPassword(string email)
    {
        var token = await GetByLoginOrEmail(email).MatchAsync(
            u => _userManager.GeneratePasswordResetTokenAsync(u),
            e => Task.FromResult("")
            );

        if (!string.IsNullOrEmpty(token))
        {
            _logger.LogInformation("Reset Token For:{email} Token:{token}", [email, token]);
        }

        return "Email Sent";
    }

}
