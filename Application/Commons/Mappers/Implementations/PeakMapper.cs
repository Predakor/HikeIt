using Application.Commons.Mappers.Interfaces;
using Application.Locations.Peaks;
using Application.Locations.Regions;
using Domain.Peaks;

namespace Application.Commons.Mappers.Implementations;

public class PeakMapper : IEntityDtoMapper<Peak, PeakDto> {
    public PeakDto MapToDto(Peak entity) {
        if (entity.Region != null) {
            return MapToCompleteDto(entity);
        }

        return new PeakDto.Simple(entity.Height, entity.Name, 1);
    }

    public static PeakDto.Complete MapToCompleteDto(Peak entity) {
        RegionDto.Complete regionDto = (RegionDto.Complete)RegionMapper.MapToDto(entity.Region);

        return new PeakDto.Complete(entity.Height, entity.Name, regionDto);
    }

}
