using Api.Extentions;
using Application.Services.Auth;
using Application.Services.Files;
using Application.Trips;
using Application.Trips.Queries;
using Application.Trips.Services;
using Domain.Common.Result;
using Domain.TripAnalytics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Dto.TripDto.Request;

namespace Api.Controllers.Trips;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase {
    readonly IAuthService _authService;
    readonly ITripService _tripService;
    readonly IGpxFileService _fileService;
    readonly ITripQueryService _queryService;
    readonly ITripAnalyticUnitOfWork _unitOfWork;

    public TripsController(
        ITripService service,
        IAuthService authService,
        IGpxFileService fileService,
        ITripQueryService queryService,
        ITripAnalyticUnitOfWork unitOfWork
    ) {
        _tripService = service;
        _fileService = fileService;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _queryService = queryService;
    }

    [HttpGet("{tripId}")]
    public async Task<IActionResult> Get(Guid tripId) {
        return await _authService
            .WithLoggedUserId()
            .BindAsync(userId => _queryService.GetWithBasicAnalytics(tripId, userId))
            .ToActionResultAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Create newTrip) {
        var ctx = CreateTripContext.Create().WithRequest(newTrip);
        return await _authService
            .WithLoggedUser()
            .MapAsync(ctx.WithUser)
            .BindAsync(_tripService.CreateSimpleAsync)
            .ToActionResultAsync();
    }

    [HttpPost("form")]
    public async Task<IActionResult> CreateWithFile([FromForm] Create newTrip, IFormFile file) {
        var ctx = CreateTripContext.Create().WithRequest(newTrip).WithFile(file);
        return await _authService
            .WithLoggedUser()
            .MapAsync(ctx.WithUser)
            .BindAsync(_ => _fileService.Validate(file))
            .BindAsync(_ => _tripService.CreateAsync(ctx))
            .MapAsync(trip => $"/trips/{trip.Id}")
            .ToActionResultAsync(ResultType.created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) {
        return await _authService
            .Me()
            .MapAsync(user => _tripService.DeleteAsync(id, user.Id))
            .BindAsync(_ => _unitOfWork.SaveChangesAsync())
            .ToActionResultAsync(ResultType.noContent);
    }
}
