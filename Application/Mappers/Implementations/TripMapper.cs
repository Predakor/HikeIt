using Application.Dto;
using Application.Mappers.Interfaces;
using Domain.Trips;

namespace Application.Mappers.Implementations;

public class TripMapper : IEntityDtoMapper<Trip, TripDto> {
    public TripDto MapToDto(Trip entity) {
        return new TripDto.Basic(entity.Height, entity.Duration, entity.Distance, entity.TripDay);
    }

    public TripDto.CompleteLinked MapToLinkedDto(Trip entity) {
        return new TripDto.CompleteLinked(
            entity.Id,
            entity.Height,
            entity.Duration,
            entity.Distance,
            entity.RegionID,
            entity.TripDay
        );
    }

    public TripDto.Complete MapToCompleteDto(Trip entity) {
        var mappedRegion = new RegionMapper().MapToCompleteDto(entity.Region);

        return new TripDto.Complete(
            entity.Id,
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
                Distance = tripDto.Duration,
            },
            TripDto.CompleteLinked tripDto => new() {
                Duration = tripDto.Duration,
                Height = tripDto.Height,
                Distance = tripDto.Duration,
                RegionID = tripDto.RegionId,
                TripDay = tripDto.TripDay,
            },
            TripDto.Complete tripDto => new() {
                Duration = tripDto.Duration,
                Height = tripDto.Height,
                Distance = tripDto.Distance,
                RegionID = tripDto.Region.Id,
                TripDay = tripDto.TripDay,
            },
            _ => throw new Exception($"Unsoported TripDto type {dto}"),
        };
    }
}
