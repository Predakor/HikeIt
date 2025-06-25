using Api.Extentions;
using Application.Services.Users;
using Domain.Entiites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase {
    readonly IUserService _userService;
    readonly UserManager<User> _userManager;

    public UsersController(IUserService service, UserManager<User> userManager) {
        _userService = service;
        _userManager = userManager;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(Guid id) =>
        await _userService.GetUserAsync(id).ToActionResultAsync();

    [HttpGet("me")]
    public async Task<IActionResult> GetMe() => await _userService.GetMe().ToActionResultAsync();
}
