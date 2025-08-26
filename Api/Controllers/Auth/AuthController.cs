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
public class AuthController : ControllerBase {
    readonly UserManager<User> _userManager;
    readonly SignInManager<User> _signInManager;
    readonly IAuthService _authService;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IAuthService authService
    ) {
        _userManager = userManager;
        _signInManager = signInManager;
        _authService = authService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me() {
        return await _authService.WithLoggedUser().BindAsync(AttachRoles).ToActionResultAsync();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto.Login dto) {
        return await _authService
            .GetByLoginOrEmail(dto.UserName)
            .BindAsync(user => TryLogin(user, dto.Password))
            .ToActionResultAsync();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto.Register dto) {
        return await ValidateEmail(dto.Email)
            .BindAsync(user => CreateUser(dto))
            .ToActionResultAsync(ResultType.created);
    }

    async Task<Result<bool>> TryLogin(User user, string password) {
        var loginAttempt = await _signInManager.PasswordSignInAsync(
            user,
            password,
            isPersistent: true,
            lockoutOnFailure: false
        );

        return loginAttempt.Succeeded ? true : Errors.InvalidCredentials();
    }

    async Task<Result<Guid>> CreateUser(UserDto.Register dto) {
        var userId = Guid.NewGuid();

        var user = new User {
            Id = userId,
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
        };

        return await CreateNewUser(user, dto.Password).BindAsync(AsignUserRole).MapAsync(u => u.Id);
    }

    async Task<Result<User>> CreateNewUser(User user, string password) {
        var result = await _userManager.CreateAsync(user, password);
        if (result.Errors.Any()) {
            return Errors.BadRequest(result.Errors.First().Description);
        }
        return user;
    }

    async Task<Result<User>> AsignUserRole(User user) {
        var result = await _userManager.AddToRoleAsync(user, "User");
        if (result.Errors.Any()) {
            return Errors.Unknown(result.Errors.First().Description);
        }
        return user;
    }

    async Task<Result<bool>> ValidateEmail(string email) {
        return await EmailValidation
            .IsValid(email)
            .BindAsync(_ => EmailValidation.IsUnique(email, _userManager));
    }

    async Task<Result<UserDto.Basic>> AttachRoles(User user) {
        var roles = await _userManager.GetRolesAsync(user);
        return user.ToBasic([.. roles]);
    }
}

internal static class EmailValidation {
    public static Result<bool> IsValid(string email) => new IsValidEmail(email).Check();

    public static async Task<Result<bool>> IsUnique(string email, UserManager<User> manager) {
        return await new IsUniqueEmail(email, manager).CheckAsync();
    }

    public class IsValidEmail(string email) : IRule {
        public string Name => "Invalid Email";
        public string Message => $"{email} is not a valid email";

        public Result<bool> Check() {
            var emailAttr = new EmailAddressAttribute();
            if (!emailAttr.IsValid(email)) {
                return Errors.RuleViolation(this);
            }

            return true;
        }
    }

    public class IsUniqueEmail(string email, UserManager<User> manager) : IRuleAsync {
        public string Name => "UniqueEmail";
        public string Message => "email must be unique";

        public async Task<Result<bool>> CheckAsync() {
            var user = await manager.FindByEmailAsync(email);

            if (user is not null) {
                return Errors.RuleViolation(this);
            }

            return true;
        }
    }
}
