using Api.Extentions;
using Application.Dto;
using Application.Peaks;
using Domain.Common.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("api/[controller]/")]
[ApiController]
public class AdminController : ControllerBase {
    readonly IPeaksService _peakService;

    public AdminController(IPeaksService peakService) {
        _peakService = peakService;
    }

    [HttpPost("peaks/add")]
    [ProducesResponseType(201)]
    public async Task<IActionResult> AddPeak(PeakDto.CreateNew request) {
        return await _peakService.Add(request).MapAsync(p => $"api/peaks/{p.Id}").ToActionResultAsync(ResultType.created);
    }

    [HttpPatch("peaks/{peakId}/update")]
    public async Task<IActionResult> UpdatePeak(int peakId, PeakDto.Update request) {
        return await _peakService.Update(peakId, request).ToActionResultAsync(ResultType.noContent);
    }
}


