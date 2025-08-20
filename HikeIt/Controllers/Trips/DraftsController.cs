using Api.Extentions;
using Application.Commons.Drafts;
using Application.Commons.FileStorage;
using Application.FileReferences;
using Application.Services.Auth;
using Application.TripAnalytics.Interfaces;
using Application.Trips;
using Application.Trips.GpxFile.Services;
using Application.Trips.Services;
using Domain.Common;
using Domain.Common.Result;
using Domain.FileReferences.ValueObjects;
using Domain.Trips;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Trips;

[Authorize]
[ApiController]
[Route("api/trips/[controller]/")]
public class DraftsController {
    readonly IGpxService _gpxService;
    readonly IAuthService _authService;
    readonly ITripService _tripService;
    readonly IGpxFileService _fileService;
    readonly IDraftService<TripDraft> _draftService;
    readonly ITripAnalyticService _analyticService;

    public DraftsController(
        IGpxService gpxService,
        IAuthService authService,
        ITripService tripService,
        IGpxFileService fileService,
        IDraftService<TripDraft> draftService,
        ITripAnalyticService tripAnalyticService
    ) {
        _gpxService = gpxService;
        _authService = authService;
        _fileService = fileService;
        _tripService = tripService;
        _draftService = draftService;
        _analyticService = tripAnalyticService;
    }

    async Task<Result<TripDraft>> GetDraft(Guid id) {
        var draft = await _draftService.Get(id);
        if (draft is null) {
            return Errors.NotFound("draft", id);
        }
        return draft;
    }

    [HttpGet("{draftId}/get")]
    public async Task<IActionResult> Get(Guid draftId) {
        return await _authService
            .WithLoggedUser()
            .MapAsync(_ => _draftService.Get(draftId))
            .ToActionResultAsync();
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

    [HttpPatch("{draftId}")]
    public async Task<IActionResult> UpdateDraft(Guid draftId, [FromBody] UpdateTripDto update) {
        return await GetDraft(draftId)
            .MapAsync(draft => draft.AddTask(PartialUpdate(update)))
            .ToActionResultAsync();
    }

    [HttpPost("{draftId}/submit")]
    public async Task<IActionResult> SubmitDraft(Guid draftId) {
        return await GetDraft(draftId)
            .MapAsync(draft => draft.AwaitAll())
            .BindAsync(_tripService.CreateAsync)
            .MapAsync(trip => $"/trips/{trip.Id}")
            .ToActionResultAsync(ResultType.created);
    }

    [HttpPost("{draftId}/file")]
    public async Task<IActionResult> AttachFile(Guid draftId, IFormFile file) {
        var draft = await _draftService.Get(draftId);

        return await ValidateAndExtractGpxFile(file)
            .BindAsync(f => CreateContext(draft, f))
            .BindAsync(ProccesGpxFile)
            .BindAsync(_analyticService.GenerateAnalytic)
            .MapAsync(draft.AddAnalytics)
            .ToActionResultAsync(ResultType.created);
    }

    static Task<Result<FileContent>> ValidateAndExtractGpxFile(IFormFile file) {
        return FileValidator.ValidateGpx(file).MapAsync(f => f.ToFileContent());
    }

    Task<Result<CreateTripContext>> CreateContext(TripDraft draft, FileContent file) {
        var ctx = CreateTripContext.Create(draft.Id).WithFile(file).WithTrip(draft.Trip);

        return _authService.WithLoggedUser().MapAsync(ctx.WithUser);
    }

    Task<Result<CreateTripContext>> ProccesGpxFile(CreateTripContext ctx) {
        return _fileService
            .CreateTemporrary(ctx.File, ctx.User.Id, ctx.Id)
            .TapAsync(file => ctx.Trip.AddGpxFile(file))
            .BindAsync(file => _gpxService.ExtractGpxData(ctx.File))
            .MapAsync(ctx.WithAnalyticData);
    }

    static Func<Trip, Task> PartialUpdate(UpdateTripDto update) {
        return async (trip) => {
            if (update.TripName is not null) {
                trip.Name = update.TripName;
            }

            if (update.TripDay.HasValue) {
                trip.SetDate(update.TripDay.Value);
            }

            if (update.RegionId.HasValue) {
                trip.ChangeRegion(update.RegionId.Value);
            }

            await Task.CompletedTask;
        };
    }
}
