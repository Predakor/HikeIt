using HikeIt.Api.Dto;
using HikeIt.Api.Entities;
using HikeIt.Api.Mappers.Interfaces;

namespace HikeIt.Api.Mappers.Implementations;

public class PeakMapper : IEntityDtoMapper<Peak, PeakDto> {
    public PeakDto MapToDto(Peak entity) {
        return new PeakDto.New(entity.Height, entity.Name);
    }

    public Peak MapToEntity(PeakDto dto) {
        return dto switch {
            PeakDto.New entity => new Peak() {
                Name = entity.Name,
                Height = entity.Height,
            },
            PeakDto.NewWithRegion entity => new Peak() {
                Name = entity.Name,
                Height = entity.Height,
                RegionID = entity.RegionId,
            },
            _ => throw new Exception("Incorect PeakDto"),
        };
    }
}
