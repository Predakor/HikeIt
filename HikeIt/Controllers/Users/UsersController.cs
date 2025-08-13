using Api.Extentions;
using Application.Services.Auth;
using Application.Users;
using Application.Users.Stats;
using Domain.Common.Result;
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

    public UsersController(
        IUserService service,
        IAuthService authService,
        IUserQueryService userQueries
    ) {
        _userService = service;
        _authService = authService;
        _userQueries = userQueries;
    }

    [HttpGet]
    public async Task<IActionResult> GetMe() => await _userService.GetMe().ToActionResultAsync();

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



    //[HttpGet]
    //[ProducesResponseType(typeof(List<TripDto.Summary>), StatusCodes.Status200OK)]
    //public async Task<IActionResult> GetAll() {
    //    return await _authService
    //        .WithLoggedUser()
    //        .BindAsync(user => _queryService.GetAllAsync(user.Id))
    //        .ToActionResultAsync();
    //}

    //[HttpGet("{id}")]
    //[ProducesResponseType(typeof(TripDto.WithBasicAnalytics), StatusCodes.Status200OK)]
    //public async Task<IActionResult> GetById(Guid id) {
    //    return await _authService
    //        .WithLoggedUser()
    //        .BindAsync(user => _queryService.GetByIdAsync(id, user.Id))
    //        .ToActionResultAsync();
    //}


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
}
