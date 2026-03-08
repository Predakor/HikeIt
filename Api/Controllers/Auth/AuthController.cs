using Api.Commons.Extentions;
using Application.Commons.Services.Auth;
using Application.Users.Root.Dtos;
using Domain.Users.Root;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IAuthService _authService;

    /// <summary>
    /// Initializes a new instance of <see cref="AuthController"/> with the required identity and authentication services.
    /// </summary>
    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IAuthService authService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
    }

    /// <summary>
    /// Gets the currently authenticated user's basic profile including assigned roles.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> containing a UserDto.Basic with the user's information and roles on success, or an error result if no authenticated user is found or retrieval fails.
    /// </returns>
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        return await _authService.WithLoggedUser().BindAsync(AttachRoles).ToActionResultAsync();
    }

    /// <summary>
    /// Authenticates a user using the provided username (or email) and password.
    /// </summary>
    /// <param name="dto">Login data containing the username or email and the password.</param>
    /// <returns>An IActionResult representing the authentication outcome: success with authentication data or an error result describing the failure.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto.Login dto)
    {
        return await _authService
            .GetByLoginOrEmail(dto.UserName)
            .BindAsync(user => TryLogin(user, dto.Password))
            .ToActionResultAsync();
    }

    /// <summary>
    /// Signs the current user out of the application.
    /// </summary>
    /// <returns>HTTP 200 OK result.</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    /// <summary>
    /// Registers a new user account using the provided registration data.
    /// </summary>
    /// <param name="dto">Registration information including username, password, first name, last name, and email.</param>
    /// <returns>201 Created with the new user's id on success; otherwise an error result describing validation or creation failure.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto.Register dto)
    {
        return await ValidateEmail(dto.Email)
            .BindAsync(user => CreateUser(dto))
            .ToActionResultAsync(ResultType.created);
    }

    /// <summary>
    /// Starts the password recovery process for the account associated with the specified email.
    /// </summary>
    /// <param name="email">The email address of the account to recover.</param>
    /// <returns>An IActionResult indicating the outcome of the forgot-password request, containing success or error details.</returns>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        return await _authService
            .ForgotPassword(email)
            .ToActionResultAsync();
    }

    /// <summary>
    /// Resets a user's password using the provided reset token and new password.
    /// </summary>
    /// <param name="dto">Reset request containing the user's identifier or email, the reset token, and the new password.</param>
    /// <returns>An IActionResult representing the outcome: 200 OK on success, or an error result with details on failure.</returns>
    [HttpPost("reset-password")]
    public Task<IActionResult> ResetPassword([FromBody] UserDto.ResetPassword dto)
    {
        return _authService.ResetPassword(dto).ToActionResultAsync();
    }

    /// <summary>
    /// Attempts to sign in the specified user using the provided password.
    /// </summary>
    /// <param name="user">The user to authenticate.</param>
    /// <param name="password">The user's password.</param>
    /// <returns>`true` if sign-in succeeded; otherwise a Result containing an invalid credentials error.</returns>
    private async Task<Result<bool>> TryLogin(User user, string password)
    {
        var loginAttempt = await _signInManager.PasswordSignInAsync(
            user,
            password,
            isPersistent: true,
            lockoutOnFailure: false
        );

        return loginAttempt.Succeeded
            ? loginAttempt.Succeeded
            : Errors.InvalidCredentials();
    }

    /// <summary>
    /// Creates a new user from the provided registration data and returns the new user's Id.
    /// </summary>
    /// <param name="dto">Registration data containing username, password, first name, last name, and email.</param>
    /// <returns>A Result containing the created user's Guid on success, or an error Result on failure.</returns>
    private async Task<Result<Guid>> CreateUser(UserDto.Register dto)
    {
        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
        };

        return await CreateNewUser(user, dto.Password).BindAsync(AsignUserRole).MapAsync(u => u.Id);
    }

    /// <summary>
    /// Creates a new user account using the provided password and returns the created user or a validation error.
    /// </summary>
    /// <param name="user">The user entity to create.</param>
    /// <param name="password">The user's password in plain text.</param>
    /// <returns>A Result containing the created <see cref="User"/> on success; a bad-request result with the first identity error description on failure.</returns>
    private async Task<Result<User>> CreateNewUser(User user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (result.Errors.Any())
        {
            return Errors.BadRequest(result.Errors.First().Description);
        }
        return user;
    }

    /// <summary>
    /// Assigns the "User" role to the specified user.
    /// </summary>
    /// <param name="user">The user to assign the "User" role to.</param>
    /// <returns>`user` wrapped in a successful Result on success, or a Result containing an unknown error with the role assignment failure description.</returns>
    private async Task<Result<User>> AsignUserRole(User user)
    {
        var result = await _userManager.AddToRoleAsync(user, "User");
        if (result.Errors.Any())
        {
            return Errors.Unknown(result.Errors.First().Description);
        }
        return user;
    }

    /// <summary>
    /// Validates that the specified email is syntactically correct and not already registered.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>A Result containing `true` if the email is valid and unique, `false` if validation failed; on failure the Result contains the validation errors.</returns>
    private async Task<Result<bool>> ValidateEmail(string email)
    {
        return await EmailValidation
            .IsValid(email)
            .BindAsync(_ => EmailValidation.IsUnique(email, _userManager));
    }

    /// <summary>
    /// Produce a basic user DTO that includes the user's assigned role names.
    /// </summary>
    /// <returns>A <see cref="UserDto.Basic"/> containing the user's data and their role names.</returns>
    private async Task<Result<UserDto.Basic>> AttachRoles(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return user.ToBasic([.. roles]);
    }
}

internal static class EmailValidation
{
    /// <summary>
/// Checks whether the provided email address is syntactically valid.
/// </summary>
/// <param name="email">The email address to validate.</param>
/// <returns>`true` if the email is a valid email address, `false` otherwise.</returns>
public static Result<bool> IsValid(string email) => new IsValidEmail(email).Check();

    /// <summary>
    /// Checks whether the specified email is not already associated with an existing user.
    /// </summary>
    /// <param name="email">The email address to validate for uniqueness.</param>
    /// <returns>`true` if the email is not already in use; otherwise a failed <see cref="Result{T}"/> containing a rule violation describing the failure.</returns>
    public static async Task<Result<bool>> IsUnique(string email, UserManager<User> manager)
    {
        return await new IsUniqueEmail(email, manager).CheckAsync();
    }

    public class IsValidEmail(string email) : IRule
    {
        public string Name => "Invalid Email";
        public string Message => $"{email} is not a valid email";

        /// <summary>
        /// Validates the stored email string and reports whether it is a valid email address.
        /// </summary>
        /// <returns>`true` if the email is valid; otherwise a rule violation result describing the invalid email.</returns>
        public Result<bool> Check()
        {
            var emailAttr = new EmailAddressAttribute();
            if (!emailAttr.IsValid(email))
            {
                return Errors.RuleViolation(this);
            }

            return true;
        }
    }

    public class IsUniqueEmail(string email, UserManager<User> manager) : IRuleAsync
    {
        public string Name => "UniqueEmail";
        public string Message => "email must be unique";

        /// <summary>
        /// Checks that the specified email is not already associated with an existing user.
        /// </summary>
        /// <returns>`true` if no user exists with the email; a rule violation error otherwise.</returns>
        public async Task<Result<bool>> CheckAsync()
        {
            var user = await manager.FindByEmailAsync(email);

            if (user is not null)
            {
                return Errors.RuleViolation(this);
            }

            return true;
        }
    }
}
