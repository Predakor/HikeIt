namespace Domain.Users.RegionProgresses.ValueObjects;

public record UpdateRegionProgress(int RegionId, IEnumerable<int> PeaksIds);
