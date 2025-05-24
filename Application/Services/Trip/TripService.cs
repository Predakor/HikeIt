using Application.Dto;
using Application.Mappers.Implementations;
using Domain.Trips;

namespace Application.Services.Trip;

public class TripService(ITripRepository tripRepository, TripMapper tripMapper) : ITripService {
    readonly ITripRepository _tripRepository = tripRepository;
    readonly TripMapper _tripMapper = tripMapper;

    public async Task<List<TripDto.Basic>> GetAll() {
        var trips = await _tripRepository.GetAllAsync();
        if (!trips.Any()) {
            return null;
        }
        var mappedTrips = trips.Select(_tripMapper.MapToBasicDto).ToList();

        return mappedTrips;
    }

    public async Task<TripDto.Complete?> GetById(int id) {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null) {
            return null;
        }

        return _tripMapper.MapToCompleteDto(trip);
    }


    public async Task<bool> Add(TripDto dto) {
        var trip = _tripMapper.MapToEntity(dto);
        await _tripRepository.AddAsync(trip);
        return true;
    }

    public async Task<bool> Delete(int id) {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null) {
            return false;
        }

        await _tripRepository.RemoveAsync(id);
        return true;
    }



    public async Task<bool> Update(TripDto.UpdateDto dto) {
        var trip = await _tripRepository.GetByIdAsync(dto.Id);
        if (trip == null) {
            return false;
        }

        await _tripRepository.UpdateAsync(dto.Id, trip);
        return true;
    }
}
