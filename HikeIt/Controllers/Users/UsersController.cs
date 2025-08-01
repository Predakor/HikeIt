using Api.Extentions;
using Application.Services.Auth;
using Application.Users;
using Application.Users.Stats;
using Domain.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase {
    readonly IUserService _userService;
    readonly IAuthService _authService;
    readonly IUserQueryService _userQueries;

    public UsersController(
        IUserService service,
        IAuthService authService,
        IUserQueryService userQueries
    ) {
        _userService = service;
        _authService = authService;
        _userQueries = userQueries;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe() => await _userService.GetMe().ToActionResultAsync();

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(Guid id) =>
        await _userService.GetUserAsync(id).ToActionResultAsync();

    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile() {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _userService.GetUserAsync(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetProfileStats() {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _userQueries.GetStats(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("regions")]
    public async Task<IActionResult> GetRegionsSummary() {
        return await _authService
            .WithLoggedUser()
            .BindAsync(u => _userQueries.GetRegionsSummaries(u.Id))
            .ToActionResultAsync();
    }
}
