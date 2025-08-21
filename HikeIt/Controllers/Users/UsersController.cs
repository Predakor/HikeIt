using Api.Extentions;
using Application.Dto;
using Application.Services.Auth;
using Application.Trips.Queries;
using Application.Users;
using Application.Users.Avatar;
using Application.Users.Dtos;
using Application.Users.Stats;
using Domain.Common.Result;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

[Authorize]
[Route("api/[controller]/me/")]
[ApiController]
public class UsersController : ControllerBase {
    readonly IUserService _userService;
    readonly IAuthService _authService;
    readonly IUserQueryService _userQueries;
    readonly ITripQueryService _tripQueries;
    readonly IUserAvatarFileService _userAvatarFileService;

    public UsersController(
        IUserService service,
        IAuthService authService,
        IUserQueryService userQueries,
        ITripQueryService tripQueries,
        IUserAvatarFileService userAvatarFileService
    ) {
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
    public async Task<IActionResult> GetUserProfile() {
        return await _authService
            .WithLoggedUserId()
            .BindAsync(_userQueries.GetProfile)
            .ToActionResultAsync();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetProfileStats() {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _userQueries.GetStats(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("trips")]
    [ProducesResponseType(typeof(List<TripDto.Summary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _tripQueries.GetAllAsync(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("trips/{id}")]
    [ProducesResponseType(typeof(TripDto.WithBasicAnalytics), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id) {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _tripQueries.GetByIdAsync(id, user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("regions")]
    public async Task<IActionResult> GetRegionsSummary() {
        return await _authService
            .WithLoggedUser()
            .BindAsync(u => _userQueries.GetRegionsSummaries(u.Id))
            .ToActionResultAsync();
    }

    [HttpGet("regions/{regionId}")]
    public async Task<IActionResult> GetRegionProgress(int regionId) {
        return await _authService
            .WithLoggedUser()
            .BindAsync(u => _userQueries.GetRegionProgess(u.Id, regionId))
            .ToActionResultAsync();
    }

    [HttpPost("data/avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile file) {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _userAvatarFileService.Upload(file, user))
            .ToActionResultAsync();
    }

    [HttpDelete("data/avatar")]
    public async Task<IActionResult> DeleteAvatar() {
        return await _authService
            .WithLoggedUser()
            .BindAsync(_userAvatarFileService.Delete)
            .ToActionResultAsync(ResultType.noContent);
    }

    [HttpPatch("data/personal")]
    public async Task<IActionResult> UpdatePersonalData(PersonalInfoUpdate update) {
        return await _authService
            .WithLoggedUser()
            .MapAsync(user => _userService.UpdatePersonalInfo(user, update))
            .ToActionResultAsync();
    }
}
