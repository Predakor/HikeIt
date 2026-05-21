using Application.Locations.Peaks;
using Application.Locations.Regions;

namespace Application.Users.RegionProgressions.Dtos;

public abstract record RegionProgressDto
{
    public sealed record Summary(
        RegionDto.Complete Region,
        short UniqueReachedPeaks,
        short TotalPeaksInRegion
    ) : RegionProgressDto;

    public sealed record Full(
        RegionDto.Complete Region,
        int TotalPeaksInRegion,
        int TotalReachedPeaks,
        int UniqueReachedPeaks,
        PeakDto.Base HighestPeak,
        PeakDto.WithReachStatus[] Peaks
    ) : RegionProgressDto;

    public sealed record TabDataLinked(
        Full Progress,
        Uri? Trips,
        Uri? Visualizations
    );
}
