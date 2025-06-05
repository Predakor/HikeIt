using Application.Services.Files;
using Application.Services.Trip;
using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using static Application.Dto.TripDto;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase {
    readonly ITripService _tripService;
    readonly IGpxFileService _fileService;

    public TripsController(ITripService service, IGpxFileService fileService) {
        _tripService = service;
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<ActionResult<Request.ResponseBasic>> GetAll() {
        var trips = await _tripService.GetAll();
        return Ok(trips);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Request.Partial>> GetById(Guid id) {
        var trip = await _tripService.GetById(id);
        if (trip is null)
            return NotFound();
        return Ok(trip);
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
        var tripResult = await _tripService.Add(newTrip);
        var savedFile = await _fileService.CreateAsync(file);

        if (savedFile.HasErrors(out Error error)) {
            return BadRequest(error);
        }

        if (tripResult.HasErrors(out error)) {
            return BadRequest(error);
        }

        var fileId = savedFile.Value.Id;
        var tripId = tripResult.Value!;
        await _tripService.UpdateGpxFile(tripId, fileId);
        return Created(string.Empty, null);
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
