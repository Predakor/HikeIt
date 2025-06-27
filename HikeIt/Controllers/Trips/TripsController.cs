using Application.Services.Auth;
using Application.Services.Files;
using Application.Services.Trips;
using Application.TripAnalytics.Interfaces;
using Domain.Common;
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
    readonly ITripAnalyticService _tripAnalyticService;

    public TripsController(
        ITripService service,
        IGpxFileService fileService,
        ITripAnalyticService tripAnalyticService,
        IAuthService authService
    ) {
        _tripService = service;
        _fileService = fileService;
        _tripAnalyticService = tripAnalyticService;
        _authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<Request.ResponseBasic>> GetAll() {
        var trips = await _tripService.GetAll();
        return Ok(trips);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Partial>> GetById(Guid id) {
        var trip = await _tripService.GetById(id);
        if (trip is null)
            return NotFound();
        return Ok(trip);
    }

    [HttpGet("{id}/analytics")]
    public async Task<ActionResult<Partial>> GetAnalytics(Guid id) {
        var trip = await _tripService.GetById(id);
        if (trip?.TrackAnalytic?.Id == null) {
            return NotFound("trip");
        }

        var analytics = await _tripAnalyticService.GetCompleteAnalytic(trip.TrackAnalytic.Id);

        if (analytics.HasErrors(out Error err)) {
            if (err is Error.NotFound) {
                return NotFound("analytics");
            }
            return BadRequest(err);
        }

        return Ok(new Partial(trip.Id, analytics.Value!, trip.GpxFile, trip.Region, trip.Base));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Request.Create newTrip) {
        await _tripService.Add(newTrip);
        return Created(string.Empty, null);
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

        var savedFile = await _fileService.CreateAsync(file, user.Id);
        if (savedFile.HasErrors(out Error error)) {
            return BadRequest(error);
        }

        var gpxData = await _fileService.GetGpxDataFromFile(file);
        if (gpxData == null || gpxData.Points.Count == 0) {
            return BadRequest("file was wrong");

        }

        var tripResult = await _tripService.Add(newTrip, gpxData, savedFile.Value.Id, user);
        if (tripResult.HasErrors(out error)) {
            Console.WriteLine("error in file tripsresult");
            Console.WriteLine("error in file tripsresult");
            Console.WriteLine("error in file tripsresult");
            return BadRequest(error);
        }

        var tripId = tripResult.Value!;

        return Created(tripId.ToString(), null);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Request.Update updateDto) {
        await _tripService.Update(updateDto);
        return NoContent();
    }

    [HttpPut("{id}/attach")]
    public async Task<IActionResult> Attach(Guid id, IFormFile file) {
        var user = (await _authService.Me()).Value;
        if (user == null) {
            return Unauthorized();
        }



        var trip = await _tripService.GetById(id);
        if (trip == null) {
            return NotFound("invalid trip id");
        }

        var savedFile = await _fileService.CreateAsync(file, user.Id);
        if (savedFile.HasErrors(out Error error)) {
            return BadRequest(error);
        }

        var fileId = savedFile.Value.Id!;

        await _tripService.UpdateGpxFile(trip.Id, fileId);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) {
        await _tripService.Delete(id);
        return NoContent();
    }
}
