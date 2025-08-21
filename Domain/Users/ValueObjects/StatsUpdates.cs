namespace Domain.Users.ValueObjects;

public static class StatsUpdates {
    public record All(Totals Totals, Locations Locations, Metas Metas);

    public record Totals(
        uint DistanceMeters,
        uint AscentMeters,
        uint DescentMeters,
        uint Peaks,
        TimeSpan Duration
    );

    public record Locations(uint UniquePeaks, uint NewRegions);

    public record Metas(uint DistanceMeters, TimeSpan? Duration);
}
