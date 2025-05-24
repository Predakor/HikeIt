using Application.Dto;
using Application.Mappers.Interfaces;
using Domain.Trips;

namespace Application.Mappers.Implementations;

public class TripMapper : IEntityDtoMapper<Trip, TripDto> {
    public TripDto MapToDto(Trip entity) {
        return new TripDto.Basic(entity.Height, entity.Distance, entity.Duration);
    }

    public TripDto.Basic MapToBasicDto(Trip entity) {
        return new TripDto.Basic(entity.Height, entity.Distance, entity.Duration);
    }

    public TripDto.Complete MapToCompleteDto(Trip entity) {
        var mappedRegion = new RegionMapper().MapToCompleteDto(entity.Region);

        return new TripDto.Complete(
            entity.Height,
            entity.Distance,
            entity.Duration,
            mappedRegion,
            entity.TripDay
        );
    }

    public Trip MapToEntity(TripDto dto) {
        return dto switch {
            TripDto.Basic tripDto => new() {
                Duration = tripDto.Duration,
                Height = tripDto.Height,
                Distance = tripDto.Length,
            },
            TripDto.CompleteLinked tripDto => new() {
                Duration = tripDto.Duration,
                Height = tripDto.Height,
                Distance = tripDto.Length,
                RegionID = tripDto.RegionId,
                TripDay = tripDto.TripDay,
            },
            TripDto.Complete tripDto => new() {
                Duration = tripDto.Duration,
                Height = tripDto.Height,
                Distance = tripDto.Length,
                RegionID = tripDto.Region.Id,
                TripDay = tripDto.TripDay,
            },
            _ => throw new Exception($"Unsoported TripDto type {dto}"),
        };
    }
}
