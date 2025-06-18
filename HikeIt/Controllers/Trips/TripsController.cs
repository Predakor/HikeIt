using Application.Services.Files;
using Application.Services.Trips;
using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using static Application.Dto.TripAnalyticsDto;
using static Application.Dto.TripDto;

namespace Api.Controllers.Trips;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase {
    readonly ITripService _tripService;
    readonly IGpxFileService _fileService;
    readonly ITripAnalyticService _tripAnalyticService;

    public TripsController(
        ITripService service,
        IGpxFileService fileService,
        ITripAnalyticService tripAnalyticService
    ) {
        _tripService = service;
        _fileService = fileService;
        _tripAnalyticService = tripAnalyticService;
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
            return NotFound();
        }

        var query = await _tripAnalyticService.GetCompleteAnalytic(trip.TrackAnalytic.Id);

        var result = query.Value;

        if (result == null) {
            return NotFound();
        }

        var gains = result.ElevationProfile?.GainsData;
        if (gains == null) {
            return BadRequest();
        }
        var scaledGains = ScaledGainSerializer.Deserialize(gains);

        static GainDto FromScaledGain(ScaledGain scaledGain) {
            return new GainDto(
                scaledGain.DistanceDelta,
                scaledGain.ElevationDelta,
                scaledGain.TimeDelta
            );
        }

        var decodedGains = scaledGains.Select(FromScaledGain).ToArray();

        var elevationDto = new ElevationProfileDto(result.ElevationProfile.Start, decodedGains);

        var analytics = new Full(
            result.RouteAnalytics ?? null,
            result.TimeAnalytics ?? null,
            result.PeaksAnalytic ?? null,
            elevationDto,
            result.Id
        );

        var tripWithAnalytic = new Partial(
            trip.Id,
            analytics,
            trip.GpxFile,
            trip.Region,
            trip.Base
        );
        return Ok(tripWithAnalytic);

        //return query.Map<ActionResult<TripAnalytic>>(
        //    analytics => Ok(analytics),
        //    notFound => NotFound(notFound.Message),
        //    error => BadRequest(error.Message)
        //);
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
        var savedFile = await _fileService.CreateAsync(file);
        if (savedFile.HasErrors(out Error error)) {
            return BadRequest(error);
        }

        var gpxData = await _fileService.GetGpxDataFromFile(file);
        if (gpxData == null || gpxData.Points.Count == 0) {
            return BadRequest("file was wrong");
        }

        var tripResult = await _tripService.Add(newTrip, gpxData, savedFile.Value.Id);
        if (tripResult.HasErrors(out error)) {
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
        var trip = await _tripService.GetById(id);
        if (trip == null) {
            return NotFound("invalid trip id");
        }

        var savedFile = await _fileService.CreateAsync(file);
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
