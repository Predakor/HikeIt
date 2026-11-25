using Api.Commons.Extentions;
using Application.Locations.Peaks;
using Application.Trips.Root.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("api/[controller]/")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IPeaksService _peakService;
    private readonly ITripQueryService _tripQueryService;

    public AdminController(IPeaksService peakService, ITripQueryService tripQueryService)
    {
        _peakService = peakService;
        _tripQueryService = tripQueryService;
    }

    // Trip picker for admin tools (e.g. the pipeline bench): search by name across every user's trips.
    [HttpGet("trips")]
    public async Task<IActionResult> SearchTrips([FromQuery] string search = "")
    {
        return await _tripQueryService
            .SearchForAdminAsync(search)
            .ToActionResultAsync();
    }

    [HttpGet("trips/{id:guid}")]
    public async Task<IActionResult> GetTrip(Guid id)
    {
        return await _tripQueryService
            .GetForAdminAsync(id)
            .ToActionResultAsync();
    }

    [HttpPost("peaks")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> AddPeak(PeakDto.CreateNew request)
    {
        return await _peakService
            .Add(request)
            .MapAsync(p => $"api/peaks/{p.Id}")
            .ToActionResultAsync(ResultType.created);
    }

    [HttpPatch("peaks/{peakId}")]
    public async Task<IActionResult> UpdatePeak(int peakId, PeakDto.Update request)
    {
        return await _peakService
            .Update(peakId, request)
            .ToActionResultAsync(ResultType.noContent);
    }

    [HttpDelete("peaks/{peakId}")]
    public async Task<IActionResult> RemovePeak(int peakId)
    {
        return await _peakService
            .Delete(peakId)
            .ToActionResultAsync(ResultType.noContent);
    }
}
