using Api.Commons.Extentions;
using Application.Locations.Peaks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("api/[controller]/")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IPeaksService _peakService;

    public AdminController(IPeaksService peakService)
    {
        _peakService = peakService;
    }

    [HttpPost("peaks")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> AddPeak(PeakDto.CreateNew request)
    {
        return await _peakService.Add(request).MapAsync(p => $"api/peaks/{p.Id}").ToActionResultAsync(ResultType.created);
    }

    [HttpPatch("peaks/{peakId}")]
    public async Task<IActionResult> UpdatePeak(int peakId, PeakDto.Update request)
    {
        return await _peakService.Update(peakId, request).ToActionResultAsync(ResultType.noContent);
    }

    [HttpDelete("peaks/{peakId}")]
    public async Task<IActionResult> RemovePeak(int peakId)
    {
        return await _peakService.Delete(peakId).ToActionResultAsync(ResultType.noContent);
    }
}

