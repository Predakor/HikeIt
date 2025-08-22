using Domain.Users.Stats;

namespace Application.Users.Stats.Dtos;

public abstract record UserStatsDto {
    public sealed record All(Totals Totals, Locations Locations, Metas Metas);

    public sealed record Totals(
        uint TotalDistanceMeters,
        uint TotalAscentMeters,
        uint TotalDescentMeters,
        TimeSpan TotalDuration,
        uint TotalPeaks,
        uint TotalTrips
    );

    public sealed record Locations(uint UniquePeaks, uint RegionsVisited);

    public sealed record Metas(
        DateOnly? FirstHikeDate,
        DateOnly? LastHikeDate,
        uint LongestTripMeters
    );
}

public static class UserStatsExtensions {
    public static UserStatsDto.All ToUserStatsDto(this UserStats stats) {
        return new UserStatsDto.All(
            stats.ToTotalsDto(),
            stats.ToLocationsDto(),
            stats.ToMetasDto()
        );
    }

    public static UserStatsDto.Totals ToTotalsDto(this UserStats stats) {
        return new UserStatsDto.Totals(
            stats.TotalDistanceMeters,
            stats.TotalAscentMeters,
            stats.TotalDescentMeters,
            stats.TotalDuration,
            stats.TotalPeaks,
            stats.TotalTrips
        );
    }

    public static UserStatsDto.Locations ToLocationsDto(this UserStats stats) {
        return new UserStatsDto.Locations(stats.UniquePeaks, stats.RegionsVisited);
    }

    public static UserStatsDto.Metas ToMetasDto(this UserStats stats) {
        return new UserStatsDto.Metas(
            stats.FirstHikeDate,
            stats.LastHikeDate,
            stats.LongestTripMeters
        );
    }
}
