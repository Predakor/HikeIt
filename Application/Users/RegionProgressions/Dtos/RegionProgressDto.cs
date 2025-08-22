using Application.Locations.Peaks;
using Application.Locations.Regions;

namespace Application.Users.RegionProgressions.Dtos;

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
