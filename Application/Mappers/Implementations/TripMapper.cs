using Application.Dto;
using Application.Mappers.Interfaces;
using Domain.Trips;

namespace Application.Mappers.Implementations;

public class TripMapper : IEntityDtoMapper<Trip, TripDto> {
    public TripDto MapToDto(Trip entity) {
        return new TripDto.Basic(entity.Height, entity.Length, entity.Duration);

    }

    public Trip MapToEntity(TripDto dto) {
        return dto switch {
            TripDto.Basic tripDto => new() {
                Duration = tripDto.Duration,
                Height = tripDto.Height,
                Length = tripDto.Length,
            },
            TripDto.CompleteLinked tripDto => new() {
                Duration = tripDto.Duration,
                Height = tripDto.Height,
                Length = tripDto.Length,
                RegionID = tripDto.RegionId,
                TripDay = tripDto.TripDay,
            },
            TripDto.Complete tripDto => new() {
                Duration = tripDto.Duration,
                Height = tripDto.Height,
                Length = tripDto.Length,
                RegionID = tripDto.Region.Id,
                TripDay = tripDto.TripDay,
            },
            _ => throw new Exception($"Unsoported TripDto type {dto}"),
        };
    }
}
