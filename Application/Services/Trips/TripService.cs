using Application.Services.Files;
using Application.TripAnalytics.Commands;
using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.Common.Result;
using Domain.Entiites.Users;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Interfaces;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Http;
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

    public async Task<Result<List<Request.ResponseBasic>>> GetAll(Guid userId) {
        return await _tripRepository
            .GetAll(userId)
            .MapAsync(trips =>
                trips
                    .Select(p => new Request.ResponseBasic(
                        p.Id,
                        p.RegionId,
                        new(p.Name, p.TripDay)
                    ))
                    .ToList()
            );
    }

    public async Task<Partial2?> GetById(Guid id, Guid userId) {
        var trip = await _unitOfWork.TripRepository.Get(id, userId);

        if (trip.Value == null) {
            return null;
        }

        return trip.Match<Partial2>(
            data =>
                new(
                    data.Id,
                    data.Analytics,
                    data.GpxFile,
                    data.Region,
                    new(data.Name, data.TripDay)
                ),
            error => null
        );
    }

    public async Task<Result<Guid>> Add(Request.Create dto, Guid userId) {
        Guid tripId = Guid.NewGuid();

        var trip = Trip.Create(tripId, dto.Base.Name, dto.Base.TripDay, userId);

        if (dto.RegionId != null) {
            trip.ChangeRegion(dto.RegionId);
        }

        _tripRepository.Add(trip);
        return Result<Guid>.Success(trip.Id);
    }

    public async Task<Result<Guid>> Add(
        Request.Create newTrip,
        AnalyticData data,
        User user,
        Guid tripId
    ) {
        var (name, tripDay) = newTrip.Base;

        var trip = Trip.Create(tripId, name, tripDay, user.Id);

        if (newTrip.RegionId != null) {
            trip.ChangeRegion(newTrip.RegionId);
        }

        trip.AddGpxFile(tripId);

        await ProccesGpxDataCommand
            .Create(data)
            .Execute()
            .BindAsync(proccesedData =>
                _analyticsService.GenerateAnalytic(proccesedData, trip, user)
            )
            .MatchAsync(
                async analytics => {
                    var result = await _unitOfWork.TripAnalytics.AddAsync(analytics);
                    trip.AddAnalytics(analytics);
                },
                error => throw new Exception(error.Message)
            );

        _unitOfWork.TripRepository.Add(trip);
        var saveChanges = await _unitOfWork.SaveChangesAsync();

        return saveChanges != null
            ? (Result<Guid>)trip.Id
            : (Result<Guid>)Errors.DbError("couldn't save");
    }

    public async Task<bool> Delete(Guid id, Guid userId) {
        var trip = await _tripRepository.Get(id, userId);
        if (trip == null) {
            return false;
        }

        return trip.Match(
            found => {
                _tripRepository.Remove(found);
                return true;
            },
            notFound => false
        );
    }

    public async Task<bool> Update(Request.Update updateDto, Guid userId) {
        var request = await _tripRepository.Get(updateDto.Id, userId);

        if (request.Value == null) {
            return false;
        }
        var trip = request.Value;

        if (updateDto.RegionId.HasValue) {
            trip.ChangeRegion(updateDto.RegionId.Value);
        }

        if (updateDto.Base != null) {
            if (updateDto.Base.TripDay != null) {
                trip.TripDay = updateDto.Base.TripDay.Value;
            }
        }
        if (updateDto.GpxFileId != null) {
            trip.AddGpxFile(updateDto.GpxFileId.Value);
        }
        ;
        return true;
    }

    public async Task<Result<Trip>> Create(Request.Create request, IFormFile file, User user) {
        Guid tripId = Guid.NewGuid();

        var (name, tripDay) = request.Base;
        var trip = Trip.Create(tripId, name, tripDay, user.Id);

        if (request.RegionId != null) {
            trip.ChangeRegion(request.RegionId);
        }

        return await ProccesFile(file)
            .BindAsync(data => GenerateAnalytics(data, tripId, user))
            .BindAsync(trip.AddAnalytics)
            .BindAsync((trip) => GenerateGpxFile(file, user.Id, tripId))
            .MapAsync(trip.AddGpxFile)
            .MapAsync(succes => _unitOfWork.SaveChangesAsync())
            .MapAsync(_ => trip);
    }

    async Task<Result<AnalyticData>> ProccesFile(IFormFile file) {
        return await _gpxFileService
            .ExtractGpxData(file)
            .BindAsync(gpxData => ProccesGpxDataCommand.Create(gpxData).Execute());
    }

    async Task<Result<TripAnalytic>> GenerateAnalytics(AnalyticData data, Guid tripId, User user) {
        return await _analyticsService
            .GenerateAnalytic(data, tripId, user)
            .MapAsync(_unitOfWork.TripAnalytics.Add);
    }

    async Task<Result<GpxFile>> GenerateGpxFile(IFormFile file, Guid userId, Guid tripId) {
        return await _gpxFileService
            .CreateAsync(file, userId, tripId)
            .MapAsync(_unitOfWork.GpxFileRepository.Add);
    }
}
