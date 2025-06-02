using Application.Dto;
using Application.Mappers.Implementations;
using Domain.Common;
using Domain.Trips;

namespace Application.Services.Trip;

public class TripService(ITripRepository tripRepository, TripMapper tripMapper) : ITripService {
    readonly ITripRepository _tripRepository = tripRepository;
    readonly TripMapper _tripMapper = tripMapper;

    //TODO!!!
    //Take id from logged user
    private static Guid GetLoggedUserId() {
        return Guid.Parse("7a4f8c5b-19b7-4a6a-89c0-f9a2e98a9380"); // Janusz
    }

    public async Task<List<TripDto.Request.ResponseBasic>> GetAll() {
        var trips = await _tripRepository.GetAllAsync();
        if (!trips.Any()) {
            return null;
        }
        var mappedTrips = trips.Select(_tripMapper.MapToBasicDto).ToList();

        return mappedTrips;
    }

    public async Task<TripDto.Partial?> GetById(Guid id) {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null) {
            return null;
        }
        return _tripMapper.MapToPartialDto(trip);
    }

    public async Task<Result<Guid>> Add(TripDto.Request.Create dto) {
        Console.WriteLine(dto.RegionId);
        var trip = _tripMapper.MapToEntity(dto);

        trip.UserId = GetLoggedUserId();
        await _tripRepository.AddAsync(trip);
        return Result<Guid>.Success(trip.Id);
    }

    public async Task<bool> Delete(Guid id) {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null) {
            return false;
        }

        await _tripRepository.RemoveAsync(id);
        return true;
    }

    public async Task<bool> Update(TripDto.Request.Update dto) {
        var trip = await _tripRepository.GetByIdAsync(dto.Id);
        if (trip == null) {
            return false;
        }

        await _tripRepository.UpdateAsync(dto.Id, trip);
        return true;
    }

    public async Task<bool> UpdateGpxFile(Guid id, Guid gpxFileId) {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null) {
            return false;
        }

        trip.AddGpxFile(gpxFileId);

        await _tripRepository.UpdateAsync(id, trip);
        return true;
    }

}
