using Application.Mappers.Implementations;
using Application.Services.Files;
using Domain.Common;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using static Application.Dto.TripDto;

namespace Application.Services.Trip;

public class TripService : ITripService {
    readonly TripMapper _tripMapper;
    readonly ITripRepository _tripRepository;
    readonly IGpxFileRepository _gpxFileRepository;
    readonly IGpxParser _gpxParser;

    public TripService(ITripRepository trips, TripMapper mapper, IGpxFileRepository files, IGpxParser gpxParser) {
        _tripRepository = trips;
        _tripMapper = mapper;
        _gpxFileRepository = files;
        _gpxParser = gpxParser;
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
        var mappedTrips = trips.Select(_tripMapper.MapToBasicDto).ToList();

        return mappedTrips;
    }

    public async Task<Partial?> GetById(Guid id) {
        var trip = await _tripRepository.GetByIdAsync(id);
        if (trip == null) {
            return null;
        }
        return _tripMapper.MapToPartialDto(trip);
    }

    public async Task<Result<Guid>> Add(Request.Create dto) {
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
            if (updateDto.Base.Distance != null) {
                trip.Distance = updateDto.Base.Distance.Value;
            }
            if (updateDto.Base.Duration != null) {
                trip.Duration = updateDto.Base.Duration.Value;
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

        var gpxStream = await _gpxFileRepository.GetGpxFileStream(gpxFileId);
        if (gpxStream == null) {
            return false;
        }

        var gpxData = await _gpxParser.ParseAsync(gpxStream);
        if (gpxData == null) {
            return false;
        }

        trip.AddGpxFile(gpxFileId);
        trip.CreateAnalytic(gpxData);

        return await _tripRepository.SaveChangesAsync();
    }
}
