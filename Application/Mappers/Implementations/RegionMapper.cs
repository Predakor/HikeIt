using Application.Dto;
using Application.Mappers.Interfaces;
using Domain.Regions;

namespace Application.Mappers.Implementations;

public class RegionMapper : IEntityDtoMapper<Region, RegionDto> {
    public RegionDto MapToDto(Region entity) {
        return new RegionDto.Complete(entity.Id, entity.Name);
    }

    public Region MapToEntity(RegionDto dto) {
        return dto switch {
            RegionDto.Basic regionDto => new() { Name = regionDto.Name },
            RegionDto.Complete regionDto => new() { Id = regionDto.Id, Name = regionDto.Name, },
            _ => throw new Exception("Invalid RegionDTO type "),
        };
    }
}
