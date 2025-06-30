using Application.Services.Files;
using Application.TripAnalytics.Commands;
using Application.TripAnalytics.Interfaces;
using Application.Trips;
using Domain.Common.Result;
using Domain.TripAnalytics.Interfaces;
using Domain.Trips;
using static Application.Dto.TripDto;

namespace Application.Services.Trips;

public class TripService : ITripService {
    readonly ITripRepository _tripRepository;
    readonly IGpxFileService _gpxFileService;
    readonly ITripAnalyticUnitOfWork _unitOfWork;
    readonly ITripAnalyticService _analyticsService;

    public TripService(
        ITripRepository trips,
        IGpxFileService gpxFileService,
        ITripAnalyticUnitOfWork unitOfWork,
        ITripAnalyticService tripAnalyticService
    ) {
        _tripRepository = trips;
        _unitOfWork = unitOfWork;
        _gpxFileService = gpxFileService;
        _analyticsService = tripAnalyticService;
    }

    public async Task<Result<Partial2>> GetByIdAsync(Guid id, Guid userId) {
        return await _unitOfWork
            .TripRepository.Get(id, userId)
            .MapAsync(TripServiceHelpers.TripToDto);
    }

    public async Task<Result<List<Request.ResponseBasic>>> GetAllAsync(Guid userId) {
        return await _tripRepository.GetAll(userId).MapAsync(TripServiceHelpers.CollectionToDtos);
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

    public async Task<Result<bool>> DeleteAsync(Guid id, Guid userId) {
        return await _tripRepository.Get(id, userId).BindAsync(_tripRepository.Remove);
    }

    async Task<Result<Trip>> SaveTripChanges(CreateTripContext ctx) {
        return await _unitOfWork.SaveChangesAsync().MapAsync(_ => ctx.Trip);
    }

    async Task<Result<CreateTripContext>> ProccesGpxFile(CreateTripContext ctx) {
        return await _gpxFileService
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
        return await _gpxFileService
            .CreateAsync(ctx.File, ctx.User.Id, ctx.Id)
            .MapAsync(_unitOfWork.GpxFileRepository.Add)
            .MapAsync(ctx.Trip.AddGpxFile)
            .MapAsync(_ => ctx);
    }

    Result<CreateTripContext> CreateTrip(CreateTripContext ctx) {
        var (name, tripDay) = ctx.Request.Base;
        var trip = Trip.Create(ctx.Id, name, tripDay, ctx.User.Id);

        if (ctx.Request.RegionId != null) {
            trip.ChangeRegion(ctx.Request.RegionId);
        }

        _unitOfWork.TripRepository.Add(trip);
        ctx.WithTrip(trip);
        return ctx;
    }
}

internal static class TripServiceHelpers {
    public static List<Request.ResponseBasic> CollectionToDtos(IEnumerable<Trip> trips) {
        return trips
            .Select(p => new Request.ResponseBasic(p.Id, p.RegionId, new(p.Name, p.TripDay)))
            .ToList();
    }

    public static Partial2 TripToDto(Trip trip) {
        return new(
            trip.Id,
            trip.Analytics,
            trip.GpxFile,
            trip.Region,
            new(trip.Name, trip.TripDay)
        );
    }
}
