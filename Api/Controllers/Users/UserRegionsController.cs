using Api.Commons.Extentions;
using Application.Commons.Services.Auth;
using Application.Users.RegionProgressions.Dtos;
using Application.Users.Regions;
using Application.Users.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

[Authorize]
[Route(RouteBase + "/regions/")]
[ApiController]
public sealed class UserRegionsController : UserControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserQueryService _userQueries;
    private readonly IUserRegionsQueries _userRegionQueries;

    private Uri GetUri(string actionName, object? values = null) => new(Url.Action(actionName, values)!, UriKind.Relative);

    public UserRegionsController(IAuthService authService, IUserQueryService userQueries, IUserRegionsQueries userRegionQueries)
    {
        _authService = authService;
        _userQueries = userQueries;
        _userRegionQueries = userRegionQueries;
    }

    [HttpGet]
    public async Task<IActionResult> GetRegionsSummary()
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(u => _userQueries.GetRegionsSummaries(u.Id))
            .ToActionResultAsync();
    }

    [HttpGet]
    [Route("{regionId}")]
    public async Task<IActionResult> GetRegionProgress(int regionId, CancellationToken ct)
    {
        return await _authService
            .WithLoggedUserId()
            .BindAsync(userId => _userRegionQueries.GetRegionProgess(userId, regionId, ct))
            .MapAsync(d =>
                new RegionProgressDto.TabDataLinked(
                    d.Progress,
                    d.HasTrips
                        ? GetUri(nameof(GetRegionTrips), new { regionId })
                        : null,
                    d.HasVisualizations
                        ? GetUri(nameof(GetRegionRoutesVisualizations), new { regionId })
                        : null
                )
            )
            .ToActionResultAsync();
    }

    [HttpGet("{regionId}/visualizations")]
    public async Task<IActionResult> GetRegionRoutesVisualizations(int regionId, CancellationToken ct)
    {
        return await _authService
            .WithLoggedUserId()
            .BindAsync(userId => _userRegionQueries.GetRouteVisualizations(userId, regionId, ct))
            .ToActionResultAsync();
    }

    [HttpGet("{regionId}/trips")]
    public async Task<IActionResult> GetRegionTrips(int regionId, CancellationToken ct)
    {
        return await _authService
            .WithLoggedUserId()
            .BindAsync(userID => _userRegionQueries.GetTrips(userID, regionId, ct))
            .ToActionResultAsync();
    }

}
