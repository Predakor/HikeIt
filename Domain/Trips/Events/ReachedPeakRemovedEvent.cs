using Domain.Interfaces;

namespace Domain.Trips.Events;

public record PeakUpdateData(int PeakId, int RegionId);

public sealed record ReachedPeakRemovedEvent(Guid UserId, PeakUpdateData[] RemovedPeaks)
    : IDomainEvent;
