using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.TripAnalytics;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using static Application.Dto.TripAnalyticsDto;

namespace Api.Controllers.Trips;

[Route("api/trips/[controller]")]
[ApiController]
public class AnalyticsController(ITripAnalyticService tripAnalyticService) : ControllerBase {
    readonly ITripAnalyticService _service = tripAnalyticService;

    [HttpGet("{id}")]
    public async Task<ActionResult<TripAnalytic>> GetById(Guid id) {
        var query = await _service.GetCompleteAnalytic(id);

        return query.Map<ActionResult<TripAnalytic>>(
            analytics => Ok(analytics),
            notFound => NotFound(notFound.Message),
            error => BadRequest(error.Message)
        );
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
}
