using Api.Commons.Extentions;
using Application.Commons.Services.Auth;
using Application.FileReferences;
using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Queries;
using Application.Trips.GpxFile.Services;
using Domain.FileReferences;
using Domain.Trips.Root.Builders.Config;
using Domain.Trips.Root.Builders.GpxDataBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Trips;

[Authorize]
[Route("api/trips/{id}/")]
[ApiController]
public class AnalyticsController : ControllerBase {
    readonly IFileReferenceRepository _fileReferenceRepository;
    readonly IElevationProfileQueryService _elevationQueries;
    readonly ITripAnalyticsQueryService _queryService;
    readonly IGpxFileService _fileService;
    readonly IAuthService _authService;
    readonly IGpxService _gpxService;

    public AnalyticsController(
        IFileReferenceRepository fileReferenceRepository,
        IElevationProfileQueryService elevationQueries,
        ITripAnalyticsQueryService queryService,
        IGpxFileService fileService,
        IAuthService authService,
        IGpxService gpxService
    ) {
        _fileReferenceRepository = fileReferenceRepository;
        _elevationQueries = elevationQueries;
        _queryService = queryService;
        _fileService = fileService;
        _authService = authService;
        _gpxService = gpxService;
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

    [HttpGet("analytics/peaks")]
    public async Task<IActionResult> GetPeakAnalytics(Guid id) {
        return await _authService
            .WithLoggedUser()
            .BindAsync(_ => _queryService.GetPeakAnalytics(id))
            .ToActionResultAsync();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("analytics/elevations/preview")]
    [ProducesResponseType(typeof(ElevationProfileDto), 200)]
    public async Task<IActionResult> DevAnalyticPreview(
        Guid id,
        [FromBody] DataProccesConfig.Partial config
    ) {
        return await _fileReferenceRepository
            .GetByIdAsync(id)
            .BindAsync(_fileService.GetAsync)
            .BindAsync(_gpxService.ExtractGpxData)
            .MapAsync(data => new ElevationDataWithConfig(data, config))
            .MapAsync(GpxDataFactory.CreateFromConfig)
            .MapAsync(x => x.ToElevationProfileDto())
            .ToActionResultAsync();
    }
}
