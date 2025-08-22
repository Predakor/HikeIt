namespace Domain.Users.RegionProgressions.ValueObjects;

public record UpdateRegionProgress(int RegionId, IEnumerable<int> PeaksIds);
