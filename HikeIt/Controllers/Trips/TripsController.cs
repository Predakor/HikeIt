using Api.Extentions;
using Application.Services.Auth;
using Application.Services.Files;
using Application.Services.Trips;
using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.Common.Result;
using Domain.TripAnalytics.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Dto.TripDto;

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
        ITripAnalyticService tripAnalyticService,
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
            .BindAsync(user => _tripService.GetAll(user.Id))
            .ToActionResultAsync();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id) {
        return await _authService
            .Me()
            .MapAsync(user => _tripService.GetById(id, user.Id))
            .ToActionResultAsync();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Request.Create newTrip) {
        return await _authService
            .Me()
            .BindAsync((user) => _tripService.Add(newTrip, user.Id))
            .ToActionResultAsync();
    }

    [HttpPost("form")]
    public async Task<IActionResult> CreateWithFile(
        [FromForm] Request.Create newTrip,
        IFormFile file
    ) {
        var user = (await _authService.Me()).Value;
        if (user == null) {
            return Unauthorized();
        }

        Guid tripId = Guid.NewGuid();

        var savedFile = await _fileService.CreateAsync(file, user.Id, tripId);
        if (savedFile.HasErrors(out Error error)) {
            return BadRequest(error);
        }

        var gpxData = await _fileService.GetGpxDataFromFile(file);
        if (gpxData == null || gpxData.Points.Count == 0) {
            return BadRequest("file was wrong");
        }

        return await _tripService
            .Add(newTrip, gpxData, user, tripId)
            .MapAsync(tripId => $"/trips/{tripId}")
            .ToActionResultAsync(ResultType.created);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) {
        return await _authService
            .Me()
            .MapAsync(user => _tripService.Delete(id, user.Id))
            .BindAsync(_ => _unitOfWork.SaveChangesAsync())
            .ToActionResultAsync(ResultType.noContent);
    }
}
