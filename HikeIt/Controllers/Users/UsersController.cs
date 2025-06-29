using Api.Extentions;
using Application.Services.Users;
using Domain.Entiites.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase {
    readonly IUserService _userService;
    readonly UserManager<User> _userManager;

    public UsersController(IUserService service, UserManager<User> userManager) {
        _userService = service;
        _userManager = userManager;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe() => await _userService.GetMe().ToActionResultAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(Guid id) =>
        await _userService.GetUserAsync(id).ToActionResultAsync();


    [HttpGet("{id}/analytics")]
    public async Task<IActionResult> GetProfileAnalytics(Guid id) {
        await Task.CompletedTask;
        return Ok();
    }

}
