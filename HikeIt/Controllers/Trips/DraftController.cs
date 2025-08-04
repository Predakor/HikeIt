using Api.Extentions;
using Application.Commons.Drafts;
using Application.Services.Auth;
using Application.Services.Files;
using Application.TripAnalytics.Interfaces;
using Application.Trips;
using Domain.Common.Result;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Trips;

[Authorize]
[ApiController]
[Route("api/trips/[controller]")]
public class DraftController {
    readonly IAuthService _authService;
    readonly IGpxFileService _fileService;
    readonly IDraftService<TripDraft> _draftService;
    readonly ITripAnalyticService _analyticService;

    public DraftController(
        IAuthService authService,
        IGpxFileService fileService,
        IDraftService<TripDraft> draftService,
        ITripAnalyticService tripAnalyticService
    ) {
        _authService = authService;
        _fileService = fileService;
        _draftService = draftService;
        _analyticService = tripAnalyticService;
    }

    [HttpPost("new")]
    public async Task<IActionResult> Create() {
        return await _authService
            .WithLoggedUserId()
            .MapAsync(TripDraft.Create)
            .MapAsync(_draftService.Add)
            .MapAsync(draft => $"/trips/drafts/{draft.Id}")
            .ToActionResultAsync(ResultType.created);
    }

    [HttpGet("/{draftId}/get")]
    public async Task<IActionResult> Get(Guid draftId) {
        return await _authService
            .WithLoggedUser()
            .MapAsync(_ => _draftService.Get(draftId))
            .ToActionResultAsync();
    }

    [HttpPut("/{draftId}/file")]
    public async Task<IActionResult> AttachFile(Guid draftId, IFormFile file) {
        var draft = await _draftService.Get(draftId);

        var ctx = CreateTripContext.Create(draftId).WithFile(file).WithTrip(draft.Trip);

        return await _authService
            .WithLoggedUser()
            .MapAsync(ctx.WithUser)
            .BindAsync(ProccesGpxFile)
            .MapAsync(ctx.WithAnalyticData)
            .BindAsync(_analyticService.GenerateAnalytic)
            .MapAsync(draft.AddAnalytics)
            .ToActionResultAsync(ResultType.created);
    }

    Task<Result<AnalyticData>> ProccesGpxFile(CreateTripContext ctx) {
        return _fileService
            .Validate(ctx.File)
            .BindAsync(file => _fileService.CreateAsync(file, ctx.User.Id, ctx.Id))
            .BindAsync(file => _fileService.ExtractGpxData(ctx.File));
    }

}
