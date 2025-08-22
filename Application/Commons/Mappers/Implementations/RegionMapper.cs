using Application.Locations.Regions;
using Domain.Locations.Regions;

namespace Application.Commons.Mappers.Implementations;

public class RegionMapper {
    public static RegionDto MapToDto(Region entity) {
        return new RegionDto.Complete(entity.Id, entity.Name);
    }
    public static RegionDto.Complete MapToCompleteDto(Region entity) {
        return new RegionDto.Complete(entity.Id, entity.Name);

    }

    public static Region MapToEntity(RegionDto dto) {
        return dto switch {
            RegionDto.Basic regionDto => new() { Name = regionDto.Name },
            RegionDto.Complete regionDto => new() { Id = regionDto.Id, Name = regionDto.Name, },
            _ => throw new Exception("Invalid RegionDTO type "),
        };
    }
}
