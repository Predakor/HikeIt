using Domain.Users.Entities;

namespace Application.Users.Stats;

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
