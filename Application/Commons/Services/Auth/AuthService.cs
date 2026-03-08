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

    /// <summary>
    /// Initializes a new instance of AuthService with the HTTP context accessor, user manager, and logger dependencies.
    /// </summary>
    public AuthService(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager, ILogger<AuthService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Resolve the currently authenticated user from the HTTP context.
    /// </summary>
    /// <returns>A Result containing the authenticated <see cref="User"/> when present; otherwise a NotAuthorized error result.</returns>
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

    /// <summary>
    /// Gets the currently authenticated user's Id from the current HTTP context.
    /// </summary>
    /// <returns>`user.Id` when a logged-in user is present; a NotAuthorized result otherwise.</returns>
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

    /// <summary>
    /// Finds a user by email or username.
    /// </summary>
    /// <param name="loginOrEmail">The user's email address or username; if the value contains '@' it is treated as an email, otherwise as a username.</param>
    /// <returns>A <see cref="Result{User}"/> containing the matched user, or a NotFound result if no user matches the provided credential.</returns>
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

    /// <summary>
    /// Attempts to reset a user's password using the provided username/email, reset token, and new password.
    /// </summary>
    /// <param name="request">Request containing the user's login (username or email), reset token, and new password.</param>
    /// <returns>`true` if the password reset operation succeeded, `false` if it failed; returns a BadRequest `Result` when `request.UserName` or `request.Password` is null.</returns>
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

    /// <summary>
    /// Generates and logs a password reset token for the user identified by the provided login or email and returns a confirmation message.
    /// </summary>
    /// <param name="email">The user's username or email address used to locate the account.</param>
    /// <returns>The confirmation message "Email Sent".</returns>
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
