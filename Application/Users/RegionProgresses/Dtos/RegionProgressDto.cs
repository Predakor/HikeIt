using Application.Dto;

namespace Application.Users.RegionProgresses.Dtos;

public abstract record RegionProgressDto {
    public sealed record Summary(
        RegionDto.Complete Region,
        short UniqueReachedPeaks,
        short TotalPeaksInRegion
    ) : RegionProgressDto;

    public sealed record Full(
        RegionDto.Complete Region,
        short TotalPeaksInRegion,
        short TotalReachedPeaks,
        short UniqueReachedPeaks,
        PeakDto.Base HighestPeak,
        PeakDto.WithReachStatus[] Peaks
    ) : RegionProgressDto;
}
