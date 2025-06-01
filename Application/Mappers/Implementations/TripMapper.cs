using Application.Dto;
using Application.Mappers.Interfaces;
using Domain.Regions;
using Domain.Trips;

namespace Application.Mappers.Implementations;

public class TripMapper : IEntityDtoMapper<Trip, TripDto> {
    public TripDto MapToDto(Trip entity) {
        TripBase tripBase = EntityToBase(entity);

        int id = entity.Id;

        if (entity.Region == null) {
            return new TripDto.Request(id, tripBase);
        }

        if (entity.GpxFile == null) {
            return new TripDto.Request(id, tripBase);
        }

        var regionDto = RegionToDto(entity.Region);
        if (entity.TrackAnalytics == null) {
            return new TripDto.Request.Response(id, regionDto, entity.GpxFile, null, tripBase);
        }

        return new TripDto.Request.ResponseFull(
            id,
            regionDto,
            entity.GpxFile,
            entity.TrackAnalytics,
            tripBase
        );
    }

    public TripDto.Request.ResponseBasic MapToBasicDto(Trip trip) {
        return new TripDto.Request.ResponseBasic(trip.Id, trip.RegionId, EntityToBase(trip));
    }

    public TripDto.Partial MapToPartialDto(Trip trip) {
        return new TripDto.Partial(
            trip.Id,
            trip.TrackAnalytics,
            trip.GpxFile,
            trip.Region,
            EntityToBase(trip)
        );
    }

    public Trip MapToEntity(TripDto dto) {
        TripBase trip = dto.Base;

        return dto switch {
            TripDto.Request.Create newTrip => new() {
                Duration = trip.Duration,
                Height = trip.Height,
                Distance = trip.Distance,
                TripDay = trip.TripDay,
                RegionId = newTrip.RegionId,
            },
            _ => throw new Exception($"Unsoported TripDto type {dto}"),
        };
    }

    static TripBase EntityToBase(Trip entity) {
        return new(entity.Height, entity.Duration, entity.Distance, entity.TripDay);
    }

    static RegionDto.Complete RegionToDto(Region region) {
        if (region == null) {
            return null;
        }
        return new RegionMapper().MapToCompleteDto(region);
    }
}
