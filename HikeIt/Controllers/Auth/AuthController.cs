using Application.Dto;
using Domain.Entiites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Auth;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {
    readonly UserManager<User> _userManager;
    readonly SignInManager<User> _signInManager;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager) {
        _userManager = userManager;
        _signInManager = signInManager;
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

        if (!result.Succeeded) {
            return BadRequest(result.Errors);
        }

        return Ok("User registered successfully");
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

        return Ok("Logged in.");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout() {
        await _signInManager.SignOutAsync();
        return Ok("Loged out");
    }
}
