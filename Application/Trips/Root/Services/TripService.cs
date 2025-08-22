using Application.FileReferences;
using Application.ReachedPeaks;
using Application.Trips.Analytics.Commands;
using Application.Trips.Analytics.Interfaces;
using Application.Trips.GpxFile.Services;
using Application.Trips.Root.ValueObjects;
using Domain.Common.Result;
using Domain.Trips.Analytics.Root.Interfaces;
using Domain.Trips.Root;

namespace Application.Trips.Root.Services;

public class TripService : ITripService {
    readonly IGpxService _gpxService;
    readonly IGpxFileService _fileService;
    readonly ITripRepository _tripRepository;
    readonly ITripAnalyticUnitOfWork _unitOfWork;
    readonly ITripAnalyticService _analyticsService;
    readonly IReachedPeaksQureryService _reachedPeaksQureryService;

    public TripService(
        IGpxService gpxService,
        IGpxFileService fileService,
        ITripRepository tripRepository,
        ITripAnalyticUnitOfWork unitOfWork,
        ITripAnalyticService analyticsService,
        IReachedPeaksQureryService reachedPeaksQureryService
    ) {
        _gpxService = gpxService;
        _fileService = fileService;
        _tripRepository = tripRepository;
        _unitOfWork = unitOfWork;
        _analyticsService = analyticsService;
        _reachedPeaksQureryService = reachedPeaksQureryService;
    }

    public async Task<Result<Guid>> CreateSimpleAsync(CreateTripContext context) {
        return await CreateTrip(context)
            .BindAsync(SaveTripChanges)
            .MapAsync(createdTrip => createdTrip.Id);
    }

    public async Task<Result<Trip>> CreateAsync(CreateTripContext ctx) {
        return await CreateTrip(ctx)
            .BindAsync(ProccesGpxFile)
            .BindAsync(CreateAnalytics)
            .BindAsync(CreateGpxFile)
            .BindAsync(SaveTripChanges);
    }

    public async Task<Result<Trip>> CreateAsync(Trip trip) {
        return await _unitOfWork
            .TripRepository.Add(trip)
            .MapAsync(async _ => {
                await _unitOfWork.SaveChangesAsync();
                return trip;
            });
    }

    public async Task<Result<bool>> DeleteAsync(Guid tripId, Guid userId) {
        var tripsReachedOnTrip = await _reachedPeaksQureryService.ReachedOnTrip(tripId);

        return await _tripRepository
            .Get(tripId, userId)
            .MapAsync(t => t.OnDelete(tripsReachedOnTrip?.Value!))
            .BindAsync(_tripRepository.Remove);
    }

    async Task<Result<Trip>> SaveTripChanges(CreateTripContext ctx) {
        return await _unitOfWork.SaveChangesAsync().MapAsync(_ => ctx.Trip);
    }

    async Task<Result<CreateTripContext>> ProccesGpxFile(CreateTripContext ctx) {
        return await _gpxService
            .ExtractGpxData(ctx.File)
            .BindAsync(data => ProccesGpxDataCommand.Create(data).Execute())
            .MapAsync(ctx.WithAnalyticData);
    }

    async Task<Result<CreateTripContext>> CreateAnalytics(CreateTripContext ctx) {
        return await _analyticsService
            .GenerateAnalytic(ctx)
            .MapAsync(_unitOfWork.TripAnalytics.Add)
            .BindAsync(ctx.Trip.AddAnalytics)
            .MapAsync(_ => ctx);
    }

    async Task<Result<CreateTripContext>> CreateGpxFile(CreateTripContext ctx) {
        return await _fileService
            .CreateTemporrary(ctx.File, ctx.User.Id, ctx.Id)
            .MapAsync(ctx.Trip.AddGpxFile)
            .MapAsync(_ => ctx);
    }

    Result<CreateTripContext> CreateTrip(CreateTripContext ctx) {
        var (name, tripDay) = ctx.Request.Base;
        var trip = Trip.Create(ctx.Id, name, tripDay, ctx.User.Id);

        if (ctx?.Request.RegionId != null) {
            trip.ChangeRegion(ctx.Request.RegionId);
        }

        return _unitOfWork.TripRepository.Add(trip).Map(ctx.WithTrip);
    }
}
