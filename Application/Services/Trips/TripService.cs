using Application.Services.Files;
using Application.TripAnalytics.Commands;
using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.Common.Result;
using Domain.TripAnalytics.Interfaces;
using Domain.Trips;
using Domain.Trips.ValueObjects;
using static Application.Dto.TripDto;

namespace Application.Services.Trips;

public class TripService : ITripService {
    readonly ITripRepository _tripRepository;
    readonly IGpxFileService _gpxFileService;
    readonly ITripAnalyticUnitOfWork _unitOfWork;
    readonly ITripAnalyticService _tripAnalyticService;

    public TripService(
        ITripRepository trips,
        IGpxFileService gpxFileService,
        ITripAnalyticUnitOfWork unitOfWork,
        ITripAnalyticService tripAnalyticService
    ) {
        _tripRepository = trips;
        _unitOfWork = unitOfWork;
        _gpxFileService = gpxFileService;
        _tripAnalyticService = tripAnalyticService;
    }

    //TODO!!!
    //Take id from logged user
    private static Guid GetLoggedUserId() {
        return Guid.Parse("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380"); // Janusz
    }

    public async Task<List<Request.ResponseBasic>> GetAll() {
        var trips = await _tripRepository.GetAllAsync();
        if (!trips.Any()) {
            return null;
        }
        var mappedTrips = trips
            .Select(p => new Request.ResponseBasic(p.Id, p.RegionId, new(p.Name, p.TripDay)))
            .ToList();

        return mappedTrips;
    }

    public async Task<Partial2?> GetById(Guid id) {
        var trip = await _unitOfWork.TripRepository.GetByIdAsync(id);
        if (trip == null) {
            return null;
        }

        Partial2 response = new(
            trip.Id,
            trip.Analytics,
            trip.GpxFile,
            trip.Region,
            new(trip.Name, trip.TripDay)
        );

        return response;
    }

    public async Task<Result<Guid>> Add(Request.Create dto) {
        var trip = Trip.Create(dto.Base.Name, dto.Base.TripDay, GetLoggedUserId(), dto.RegionId);

        await _tripRepository.AddAsync(trip);
        return Result<Guid>.Success(trip.Id);
    }

    public async Task<Result<Guid>> Add(Request.Create newTrip, AnalyticData data, Guid fileId) {
        var trip = Trip.Create(
            newTrip.Base.Name,
            newTrip.Base.TripDay,
            GetLoggedUserId(),
            newTrip.RegionId
        );
        trip.AddGpxFile(fileId);

        ProccesGpxDataCommand
            .Create(data)
            .Execute()
            .Match(
                async proccesedData => {
                    var generatedAnalytics = await _tripAnalyticService.GenerateAnalytic(
                        proccesedData,
                        trip
                    );
                    generatedAnalytics.Match(
                        analytics => trip.AddAnalytics(analytics),
                        error => { }
                    );
                },
                e => throw new Exception(e.Message)
            );

        await _tripRepository.AddAsync(trip);
        var saveChanges = await _unitOfWork.SaveChangesAsync();

        return saveChanges != null
            ? (Result<Guid>)trip.Id
            : (Result<Guid>)Errors.DbError("couldn't save");
    }

    public async Task<bool> Delete(Guid id) {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null) {
            return false;
        }

        await _tripRepository.RemoveAsync(id);
        return true;
    }

    public async Task<bool> Update(Request.Update updateDto) {
        var trip = await _tripRepository.GetByIdAsync(updateDto.Id);
        if (trip == null) {
            return false;
        }

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

        await _tripRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateGpxFile(Guid id, Guid gpxFileId) {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null) {
            return false;
        }

        var gpxStream = await _gpxFileService.GetByIdAsync(gpxFileId);
        if (gpxStream == null) {
            return false;
        }

        trip.AddGpxFile(gpxFileId);

        return await _tripRepository.SaveChangesAsync();
    }
}
