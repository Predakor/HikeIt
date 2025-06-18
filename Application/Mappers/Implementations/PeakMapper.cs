using Application.Dto;
using Application.Mappers.Interfaces;
using Domain.Entiites.Peaks;

namespace Application.Mappers.Implementations;

public class PeakMapper : IEntityDtoMapper<Peak, PeakDto> {
    public PeakDto MapToDto(Peak entity) {
        if (entity.Region != null) {
            return MapToCompleteDto(entity);
        }

        return new PeakDto.Simple(entity.Height, entity.Name, 1);
    }

    public PeakDto.Complete MapToCompleteDto(Peak entity) {
        RegionDto.Complete regionDto = (RegionDto.Complete)new RegionMapper().MapToDto(entity.Region);

        return new PeakDto.Complete(entity.Height, entity.Name, regionDto);
    }


}
