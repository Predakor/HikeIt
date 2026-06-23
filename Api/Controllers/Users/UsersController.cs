using Api.Commons.Extentions;
using Application.Commons.Services.Auth;
using Application.Trips.Root.Dtos;
using Application.Trips.Root.Queries;
using Application.Users.Avatar;
using Application.Users.Root;
using Application.Users.Root.Dtos;
using Application.Users.Stats;
using Domain.Users.Root.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

[Authorize]
[Route(RouteBase)]
[ApiController]
public class UsersController : UserControllerBase
{

    private readonly IUserQueryService _userQueries;
    private readonly ITripQueryService _tripQueries;
    private readonly IUserAvatarFileService _userAvatarFileService;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UsersController(
        IUserService service,
        IAuthService authService,
        IUserQueryService userQueries,
        ITripQueryService tripQueries,
        IUserAvatarFileService userAvatarFileService
    )
    {
        _userService = service;
        _authService = authService;
        _userQueries = userQueries;
        _tripQueries = tripQueries;
        _userAvatarFileService = userAvatarFileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMe() => await _userService.GetMe().ToActionResultAsync();

    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserDto.Profile), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserProfile(CancellationToken ct)
    {
        return await _authService
            .WithLoggedUserId()
            .BindAsync(user => _userQueries.GetProfile(user, ct))
            .ToActionResultAsync();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetProfileStats()
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _userQueries.GetStats(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("trips")]
    [ProducesResponseType(typeof(List<TripDto.Summary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _tripQueries.GetSummariesAsync(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("trips/{id}")]
    [ProducesResponseType(typeof(TripDto.WithBasicAnalytics), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id)
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _tripQueries.GetByIdAsync(id, user.Id))
            .ToActionResultAsync();
    }

    [HttpPost("data/avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _userAvatarFileService.Upload(file, user))
            .ToActionResultAsync();
    }

    [HttpDelete("data/avatar")]
    public async Task<IActionResult> DeleteAvatar()
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(_userAvatarFileService.Delete)
            .ToActionResultAsync(ResultType.noContent);
    }

    [HttpPatch("data/personal")]
    public async Task<IActionResult> UpdatePersonalData(PersonalInfoUpdate update)
    {
        return await _authService
            .WithLoggedUser()
            .MapAsync(user => _userService.UpdatePersonalInfo(user, update))
            .ToActionResultAsync();
    }
}
