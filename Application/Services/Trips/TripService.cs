using Application.Services.Files;
using Application.TripAnalytics.Commands;
using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.TripAnalytics;
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
        ITripAnalyticService tripAnalyticService,
        ITripAnalyticUnitOfWork unitOfWork
    ) {
        _tripRepository = trips;
        _gpxFileService = gpxFileService;
        _unitOfWork = unitOfWork;
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

    public async Task<Partial?> GetById(Guid id) {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null) {
            return null;
        }
        TripAnalytic? analytic = null;

        if (trip.TripAnalyticId != null) {
            analytic = await _tripAnalyticService.GetAnalytic((Guid)trip.TripAnalyticId!);
        }

        Partial response = new(
            trip.Id,
            analytic,
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

    public async Task<Result<Guid>> Add(Request.Create newTrip, AnalyticData data) {
        var trip = Trip.Create(
            newTrip.Base.Name,
            newTrip.Base.TripDay,
            GetLoggedUserId(),
            newTrip.RegionId
        );

        ProccesGpxDataCommand
            .Create(data)
            .Execute()
            .Match(
                async proccesedData => {
                    var analytics = await _tripAnalyticService.GenerateAnalytic(proccesedData);
                    if (analytics != null) {
                        trip.AddAnalytics(analytics);
                    }
                },
                e => throw new Exception(e.Message)
            );

        await _tripRepository.AddAsync(trip);
        var saveChanges = await _unitOfWork.SaveChangesAsync();
        return saveChanges.Map(
            succes => Result<Guid>.Success(trip.Id),
            error => Result<Guid>.Failure(error)
        );
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
