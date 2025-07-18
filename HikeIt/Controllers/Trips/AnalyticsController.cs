using Api.Extentions;
using Application.Dto.Analytics;
using Application.Services.Auth;
using Application.Services.Files;
using Application.TripAnalytics.Interfaces;
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
    readonly ITripAnalyticsQueryService _queryService;
    readonly ITripAnalyticService _service;
    readonly IGpxFileService _fileService;
    readonly IAuthService _authService;

    public AnalyticsController(
        ITripAnalyticService tripAnalyticService,
        ITripAnalyticsQueryService queryService,
        IGpxFileService fileService,
        IAuthService authService
    ) {
        _service = tripAnalyticService;
        _queryService = queryService;
        _fileService = fileService;
        _authService = authService;
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalytics(Guid id) {
        return await _authService
            .Me()
            .BindAsync(_ => _queryService.GetCompleteAnalytics(id))
            .ToActionResultAsync();
    }

    [HttpGet("analytics/elevation")]
    public async Task<ActionResult<ElevationProfileDto>> GetElevationProfile(Guid id) {
        var query = await _service.GetElevationProfile(id);

        if (query == null) {
            return NotFound();
        }

        var scaledGains = ScaledGainSerializer.Deserialize(query.GainsData);

        static GainDto FromScaledGain(ScaledGain scaledGain) {
            return new GainDto(
                scaledGain.DistanceDelta,
                scaledGain.ElevationDelta,
                scaledGain.TimeDelta
            );
        }

        var gains = scaledGains.Select(FromScaledGain).ToArray();
        return Ok(new ElevationProfileDto(query.Start, gains));
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
