using Application.Dto;
using Application.Mappers.Interfaces;
using Domain.Peaks;

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

    public Peak MapToEntity(PeakDto dto) {
        return dto switch {
            PeakDto.Simple entity => new Peak() {
                Name = entity.Name,
                Height = entity.Height,
            },
            PeakDto.Complete entity => new Peak() {
                Name = entity.Name,
                Height = entity.Height,
                RegionID = entity.Region.Id,
            },
            _ => throw new Exception("Incorect PeakDto"),
        };
    }
}
