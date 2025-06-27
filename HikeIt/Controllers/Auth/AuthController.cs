using Application.Dto;
using Application.Services.Auth;
using Domain.Entiites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto.Register dto) {
        var user = new User {
            UserName = dto.UserName,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        return result.Succeeded ? Ok(result) : BadRequest(result.Errors);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto.Login dto) {
        var result = await _signInManager.PasswordSignInAsync(
            dto.UserName,
            dto.Password,
            isPersistent: true,
            lockoutOnFailure: false
        );

        if (!result.Succeeded) {
            return Unauthorized("Invalid credentials.");
        }

        return Ok();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me() {
        var query = await _authService.Me();

        return query.Match<IActionResult>(
            user => Ok(UserDtoFactory.CreateBasic(user)),
            error => Unauthorized()
        );
    }
}
