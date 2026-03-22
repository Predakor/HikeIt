using Api.Commons.Extentions;
using Application.Commons.Abstractions;
using Application.Commons.Services.Auth;
using Application.FileReferences;
using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Queries;
using Application.Trips.Analytics.RouteVisualizations;
using Application.Trips.GpxFile.Services;
using Domain.FileReferences;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Root.Builders.Config;
using Domain.Trips.Root.Builders.GpxDataBuilder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Trips;

[Authorize]
[Route("api/trips/{id}/analytics/")]
[ApiController]
public class AnalyticsController : ControllerBase
{
    private readonly IRouteVisualizationService _routeVisualizationService;
    private readonly IFileReferenceRepository _fileReferenceRepository;
    private readonly IElevationProfileQueryService _elevationQueries;
    private readonly ITripAnalyticsQueryService _analyticsQueryies;
    private readonly IGpxFileService _fileService;
    private readonly IAuthService _authService;
    private readonly IGpxService _gpxService;

    public AnalyticsController(
        IRouteVisualizationService routeVisualizationService,
        IFileReferenceRepository fileReferenceRepository,
        IElevationProfileQueryService elevationQueries,
        ITripAnalyticsQueryService queryService,
        IGpxFileService fileService,
        IAuthService authService,
        IGpxService gpxService
    )
    {
        _routeVisualizationService = routeVisualizationService;
        _fileReferenceRepository = fileReferenceRepository;
        _elevationQueries = elevationQueries;
        _analyticsQueryies = queryService;
        _fileService = fileService;
        _authService = authService;
        _gpxService = gpxService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAnalytics(Guid id)
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(_ => _analyticsQueryies.GetCompleteAnalytics(id))
            .ToActionResultAsync();
    }

    [HttpGet("elevation")]
    public async Task<IActionResult> GetElevationProfile(Guid id)
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(_ => _elevationQueries.GetElevationProfile(id))
            .ToActionResultAsync();
    }

    [HttpGet("peaks")]
    public async Task<IActionResult> GetPeakAnalytics(Guid id)
    {
        return await _authService
            .WithLoggedUser()
            .BindAsync(_ => _analyticsQueryies.GetPeakAnalytics(id))
            .ToActionResultAsync();
    }

    [HttpGet("visualisation")]
    public Task<IActionResult> GetVisualisationPath(Guid id)
    {
        return _authService
            .WithLoggedUser()
            .BindAsync(_ => _analyticsQueryies.GetRouteVisualisation(id))
            .ToActionResultAsync();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("elevations/preview")]
    [ProducesResponseType(typeof(ElevationProfileDto), 200)]
    public async Task<IActionResult> DevAnalyticPreview(
        Guid id,
        [FromBody] DataProccesConfig.Partial config
    )
    {
        return await _fileReferenceRepository
            .GetByIdAsync(id)
            .BindAsync(_fileService.GetAsync)
            .BindAsync(_gpxService.ExtractGpxData)
            .MapAsync(data => new ElevationDataWithConfig(data, config))
            .MapAsync(GpxDataFactory.CreateFromConfig)
            .MapAsync(x => x.ToElevationProfileDto())
            .ToActionResultAsync();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("visualizations/preview")]
    [ProducesResponseType(typeof(RoutePath), 200)]
    public async Task<IActionResult> DevAnalyticPreview(Guid id, [FromBody] IEnumerable<IFilterConfig> configs)
    {
        return await _fileReferenceRepository
            .GetByIdAsync(id)
            .BindAsync(_fileService.GetAsync)
            .BindAsync(_gpxService.ExtractGpxData)
            .BindAsync(d => _routeVisualizationService.GetRouteVisualization(d.Points, configs))
            .ToActionResultAsync();

    }
}
