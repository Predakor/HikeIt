using Api.Extentions;
using Application.Dto.Analytics;
using Application.Services.Auth;
using Application.Services.Files;
using Application.TripAnalytics.ElevationProfiles;
using Application.TripAnalytics.Quries;
using Domain.Common;
using Domain.Common.Result;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.Config;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Trips;

[Route("api/trips/{id}/")]
[ApiController]
public class AnalyticsController : ControllerBase {
    readonly IElevationProfileQueryService _elevationQueries;
    readonly ITripAnalyticsQueryService _queryService;
    readonly IGpxFileService _fileService;
    readonly IAuthService _authService;

    public AnalyticsController(
        IElevationProfileQueryService elevationQueries,
        ITripAnalyticsQueryService queryService,
        IGpxFileService fileService,
        IAuthService authService
    ) {
        _elevationQueries = elevationQueries;
        _queryService = queryService;
        _fileService = fileService;
        _authService = authService;
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalytics(Guid id) {
        return await _authService
            .WithLoggedUser()
            .BindAsync(_ => _queryService.GetCompleteAnalytics(id))
            .ToActionResultAsync();
    }

    [HttpGet("analytics/elevation")]
    public async Task<IActionResult> GetElevationProfile(Guid id) {
        return await _authService
            .WithLoggedUser()
            .BindAsync(_ => _elevationQueries.GetElevationProfile(id))
            .ToActionResultAsync();
    }

    [HttpPost("analytics/elevations/preview")]
    public async Task<ActionResult<ElevationProfileDto?>> DevAnalyticPreview(
        Guid id,
        [FromBody] ConfigBase.Nullable config
    ) {
        var file = await _fileService.ExtractGpxData(id);

        if (file == null) {
            return NotFound("File not found");
        }

        static GainDto ToGainDto(GpxGain g) {
            return new GainDto(
                g.DistanceDelta,
                g.ElevationDelta,
                g.TimeDelta.HasValue ? g.TimeDelta.Value : 0
            );
        }

        if (file.HasErrors(out var error)) {
            return BadRequest(error);
        }

        var data = file.Value!;

        ElevationDataWithConfig eleData = new(new(data), config);
        var eleDataFromConfig = GpxDataFactory.CreateFromConfig(eleData);
        var gains = eleDataFromConfig.Gains ?? eleDataFromConfig.Points.ToGains();

        var gainsDtos = gains.Select(ToGainDto).ToArray();

        var eleProfileDto = new ElevationProfileDto(data.Points[0], gainsDtos);
        return Ok(eleProfileDto);
    }
}
