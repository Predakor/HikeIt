using Api.Extentions;
using Application.Services.Auth;
using Application.Services.Users;
using Domain.Common.Result;
using Domain.Entiites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase {
    readonly IUserService _userService;
    readonly IAuthService _authService;
    readonly UserManager<User> _userManager;

    public UsersController(
        IUserService service,
        UserManager<User> userManager,
        IAuthService authService
    ) {
        _userService = service;
        _userManager = userManager;
        _authService = authService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe() => await _userService.GetMe().ToActionResultAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(Guid id) =>
        await _userService.GetUserAsync(id).ToActionResultAsync();

    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile() {
        return await _authService
            .Me()
            .BindAsync(user => _userService.GetUserAsync(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("{id}/analytics")]
    public async Task<IActionResult> GetProfileAnalytics(Guid id) {
        await Task.CompletedTask;
        return Ok();
    }
}
