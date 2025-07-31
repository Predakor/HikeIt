using Application.Dto;

namespace Application.Users.RegionProgress.Dtos;

public abstract record RegionProgressDto {
    public sealed record Summary(RegionDto.Complete Region, short ReachedPeaks, short TotalPeaks)
        : RegionProgressDto;
}
