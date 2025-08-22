using Api.Commons.Extentions;
using Application.Commons.Files;
using Application.Commons.Services.Auth;
using Application.FileReferences;
using Application.Trips.Root.Queries;
using Application.Trips.Root.Services;
using Application.Trips.Root.ValueObjects;
using Domain.Trips.Analytics.Root.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Trips.Root.Dtos.TripDto.Request;

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
        var ctx = CreateTripContext.Create().WithRequest(newTrip);
        return await _authService
            .WithLoggedUser()
            .MapAsync(ctx.WithUser)
            .BindAsync(_ => FileValidator.ValidateGpx(file))
            .MapAsync(file => file.ToFileContent())
            .MapAsync(ctx.WithFile)
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
