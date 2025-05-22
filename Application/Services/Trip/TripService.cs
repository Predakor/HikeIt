using Application.Dto;
using Application.Mappers.Implementations;
using Domain.Trips;

namespace Application.Services.Trip;

public interface ITripService {
    public Task<bool> Add(TripDto dto);
    public Task<bool> Update(TripDto.UpdateDto dto);
    public Task<bool> Delete(int id);

    public Task<TripDto?> GetById(int id);
    public Task<List<TripDto>> GetAll();
}

public class TripService(ITripRepository tripRepository, TripMapper tripMapper) : ITripService {
    readonly ITripRepository _tripRepository = tripRepository;
    readonly TripMapper _tripMapper = tripMapper;

    public async Task<List<TripDto>> GetAll() {
        var trips = await _tripRepository.GetAllAsync();
        if (trips.Count == 0) {
            return null;
        }
        var mappedTrips = trips.Select(_tripMapper.MapToDto).ToList();

        return mappedTrips;
    }

    public async Task<TripDto?> GetById(int id) {
        var trip = await _tripRepository.GetAsync(id);
        if (trip == null) {
            return null;
        }

        return _tripMapper.MapToDto(trip);
    }


    public async Task<bool> Add(TripDto dto) {
        var trip = _tripMapper.MapToEntity(dto);
        await _tripRepository.Add(trip);
        return true;
    }

    public async Task<bool> Delete(int id) {
        var trip = await _tripRepository.GetAsync(id);
        if (trip == null) {
            return false;
        }

        await _tripRepository.Delete(trip);
        return true;
    }



    public async Task<bool> Update(TripDto.UpdateDto dto) {
        var trip = await _tripRepository.GetAsync(dto.Id);
        if (trip == null) {
            return false;
        }

        await _tripRepository.Update(trip);
        return true;
    }
}
