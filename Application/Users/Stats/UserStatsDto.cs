namespace Application.Users.Stats;

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
