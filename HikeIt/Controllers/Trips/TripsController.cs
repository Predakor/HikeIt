using Api.Extentions;
using Application.Services.Auth;
using Application.Services.Files;
using Application.Services.Trips;
using Application.Trips;
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

    public TripsController(
        ITripService service,
        IGpxFileService fileService,
        IAuthService authService,
        ITripAnalyticUnitOfWork unitOfWork
    ) {
        _tripService = service;
        _fileService = fileService;
        _authService = authService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        return await _authService
            .Me()
            .BindAsync(user => _tripService.GetAllAsync(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id) {
        return await _authService
            .Me()
            .MapAsync(user => _tripService.GetByIdAsync(id, user.Id))
            .ToActionResultAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Create newTrip) {
        var ctx = CreateTripContext.Create().WithRequest(newTrip);
        return await _authService
            .Me()
            .BindAsync((user) => _tripService.CreateSimpleAsync(ctx))
            .ToActionResultAsync();
    }

    [HttpPost("form")]
    public async Task<IActionResult> CreateWithFile([FromForm] Create newTrip, IFormFile file) {
        var ctx = CreateTripContext.Create().WithRequest(newTrip).WithFile(file);
        return await _authService
            .Me()
            .MapAsync(ctx.WithUser)
            .BindAsync(_ => _fileService.Validate(file))
            .BindAsync(_ => _tripService.CreateAsync(ctx))
            .MapAsync(trip => $"/trips/{trip.Id}")
            .ToActionResultAsync();
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
