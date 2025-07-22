using Application.Dto;
using Application.Mappers.Interfaces;
using Domain.Mountains.Peaks;

namespace Application.Mappers.Implementations;

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

    public static PeakDto.WithLocation ToWithLocation(Peak peak) {
        return new PeakDto.WithLocation(
            peak.Height,
            peak.Name ?? "",
            peak.Location.Y,
            peak.Location.X
        );
    }
}
