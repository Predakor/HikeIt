using Api.Extentions;
using Application.Commons.Drafts;
using Application.Services.Auth;
using Application.Services.Files;
using Application.TripAnalytics.Interfaces;
using Application.Trips;
using Application.Trips.Services;
using Domain.Common;
using Domain.Common.Result;
using Domain.Trips;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Trips;

[Authorize]
[ApiController]
[Route("api/trips/[controller]/")]
public class DraftsController {
    readonly IAuthService _authService;
    readonly ITripService _tripService;
    readonly IGpxFileService _fileService;
    readonly IDraftService<TripDraft> _draftService;
    readonly ITripAnalyticService _analyticService;

    public DraftsController(
        IAuthService authService,
        ITripService tripService,
        IGpxFileService fileService,
        IDraftService<TripDraft> draftService,
        ITripAnalyticService tripAnalyticService
    ) {
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

        return await CreateContext(draft, file)
            .BindAsync(ProccesGpxFile)
            .BindAsync(_analyticService.GenerateAnalytic)
            .MapAsync(draft.AddAnalytics)
            .ToActionResultAsync(ResultType.created);
    }

    Task<Result<CreateTripContext>> CreateContext(TripDraft draft, IFormFile file) {
        var ctx = CreateTripContext.Create(draft.Id).WithFile(file).WithTrip(draft.Trip);

        return _authService.WithLoggedUser().MapAsync(ctx.WithUser);
    }

    Task<Result<CreateTripContext>> ProccesGpxFile(CreateTripContext ctx) {
        return _fileService
            .Validate(ctx.File)
            .BindAsync(file => _fileService.CreateAsync(file, ctx.User.Id, ctx.Id))
            .TapAsync(file => ctx.Trip.AddGpxFile(file))
            .BindAsync(file => _fileService.ExtractGpxData(ctx.File))
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
