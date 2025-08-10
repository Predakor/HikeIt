using Api.Extentions;
using Application.Dto;
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
    readonly ITripAnalyticUnitOfWork _unitOfWork;
    readonly ITripQueryService _queryService;

    public TripsController(
        ITripService service,
        IGpxFileService fileService,
        IAuthService authService,
        ITripAnalyticUnitOfWork unitOfWork,
        ITripQueryService queryService
    ) {
        _tripService = service;
        _fileService = fileService;
        _authService = authService;
        _unitOfWork = unitOfWork;
        _queryService = queryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TripDto.Summary>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll() {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _queryService.GetAllAsync(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TripDto.WithBasicAnalytics), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(Guid id) {
        return await _authService
            .WithLoggedUser()
            .BindAsync(user => _queryService.GetByIdAsync(id, user.Id))
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
