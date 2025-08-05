using Api.Extentions;
using Application.Commons.Drafts;
using Application.Services.Auth;
using Application.Services.Files;
using Application.Services.Trips;
using Application.TripAnalytics.Interfaces;
using Application.Trips;
using Domain.Common;
using Domain.Common.Result;
using Domain.Trips.ValueObjects;
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
    public async Task<IActionResult> UpdateDraft(Guid draftId, [FromBody] UpdateTripDto dto) {
        var draft = await _draftService.Get(draftId);

        if (dto.TripName is not null) {
            draft.Trip.Name = dto.TripName;
        }

        if (dto.TripDay.HasValue) {
            draft.Trip.TripDay = dto.TripDay.Value;
        }

        Console.WriteLine(dto.RegionId);
        Console.WriteLine(dto.RegionId);
        Console.WriteLine(dto.RegionId);
        Console.WriteLine(dto.RegionId);
        Console.WriteLine(dto.RegionId);


        if (dto.RegionId is not null) {

            draft.Trip.ChangeRegion(dto.RegionId.Value);
        }

        return new OkObjectResult(draftId);
    }

    [HttpPost("{draftId}/submit")]
    public async Task<IActionResult> SubmitDraft(Guid draftId) {
        return await GetDraft(draftId)
            .BindAsync(_tripService.CreateAsync)
            .MapAsync(trip => $"/trips/{trip.Id}")
            .ToActionResultAsync(ResultType.created);
    }

    [HttpPost("{draftId}/file")]
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
