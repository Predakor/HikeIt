using Application.Dto;
using Application.Services.Files;
using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.TripAnalytics;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.Config;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Trips;

[Route("api/trips/[controller]")]
[ApiController]
public class AnalyticsController : ControllerBase {
    readonly ITripAnalyticService _service;
    readonly IGpxFileService _fileService;

    public AnalyticsController(
        ITripAnalyticService tripAnalyticService,
        IGpxFileService fileService
    ) {
        _service = tripAnalyticService;
        _fileService = fileService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TripAnalytic>> GetById(Guid id) {
        var query = await _service.GetCompleteAnalytic(id);

        if (query == null) {
            return NotFound("no analytics found");
        }

        if (query.HasErrors(out var error)) {
            return BadRequest(error);
        }
        return Ok(query);
    }

    //point to point gains/deltas

    [HttpGet("elevations/{id}")]
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

    [HttpPost("elevations/{fileId}/preview")]
    public async Task<ActionResult<ElevationProfileDto?>> DevAnalyticPreview(
        Guid fileId,
        [FromBody] ConfigBase.Nullable config
    ) {
        var file = await _fileService.GetGpxDataByFileIdAsync(fileId);

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
        var eleDataFromConfig = GpxDataFactory.Create(eleData);
        var gains = eleDataFromConfig.Gains ?? eleDataFromConfig.Points.ToGains();

        var gainsDtos = gains.Select(ToGainDto).ToArray();

        var eleProfileDto = new ElevationProfileDto(data.Points[0], gainsDtos);
        return Ok(eleProfileDto);
    }
}
